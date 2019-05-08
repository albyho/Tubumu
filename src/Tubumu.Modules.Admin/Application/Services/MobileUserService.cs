using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Admin.Domain.Services;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Admin.Settings;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Sms;

namespace Tubumu.Modules.Admin.Application.Services
{
    public interface IMobileUserService
    {
        Task<bool> IsExistsMobileAsync(string mobile);

        Task<bool> VerifyExistsMobileAsync(int userId, string mobile);

        Task<bool> ChangeMobileAsync(int userId, string newMobile, bool mobileIsValid, ModelStateDictionary modelState);

        Task<UserInfo> GenerateItemAsync(Guid groupId, UserStatus status, MobilePassswordValidationCodeRegisterInput input, ModelStateDictionary modelState);

        Task<bool> ResetPasswordAsync(MobileResetPassswordInput input, ModelStateDictionary modelState);

        Task<UserInfo> GetItemByMobileAsync(string mobile, bool mobileIsValid = true, UserStatus? status = null);

        Task<UserInfo> GetOrGenerateItemByMobileAsync(Guid groupId, UserStatus generateStatus, string mobile, bool mobileIsValid, ModelStateDictionary modelState);

        Task<bool> GetMobileValidationCodeAsync(GetMobileValidationCodeInput getMobileValidationCodeInput, ModelStateDictionary modelState);

        Task<bool> VerifyMobileValidationCodeAsync(VerifyMobileValidationCodeInput verifyMobileValidationCodeInput, ModelStateDictionary modelState, string defaultCode = null);

        Task<bool> FinishVerifyMobileValidationCodeAsync(string mobile, MobileValidationCodeType type, ModelStateDictionary modelState);
    }

    public class MobileUserService : IMobileUserService
    {
        private const string MobileValidationCodeCacheKeyFormat = "MobileValidationCode:{0}";

        private readonly IMobileUserManager _manager;
        private readonly IDistributedCache _cache;
        private readonly ISmsSender _smsSender;
        private readonly MobileValidationCodeSettings _mobileValidationCodeSettings;
        private readonly ILogger<MobileUserService> _logger;

        public MobileUserService(IMobileUserManager manager,
            IDistributedCache cache,
            ISmsSender smsSender,
            IOptions<MobileValidationCodeSettings> mobileValidationCodeSettingsOptions,
            ILogger<MobileUserService> logger
            )
        {
            _mobileValidationCodeSettings = mobileValidationCodeSettingsOptions.Value;
            _cache = cache;
            _smsSender = smsSender;
            _manager = manager;
            _logger = logger;
        }

        #region IMobileUserService Members

        public Task<bool> IsExistsMobileAsync(string mobile)
        {
            if (mobile.IsNullOrWhiteSpace()) return Task.FromResult(false);
            return _manager.IsExistsMobileAsync(mobile);
        }

        public Task<bool> VerifyExistsMobileAsync(int userId, string mobile)
        {
            if (mobile.IsNullOrWhiteSpace()) return Task.FromResult(false);
            return _manager.VerifyExistsMobileAsync(userId, mobile);
        }

        public async Task<bool> ChangeMobileAsync(int userId, string newMobile, bool mobileIsValid, ModelStateDictionary modelState)
        {
            bool result = await _manager.ChangeMobileAsync(userId, newMobile, mobileIsValid, modelState);
            if (!result)
            {
                modelState.AddModelError("UserId", "修改手机号失败，可能当前用户不存在或新手机号已经被使用");
            }
            else
            {
                CleanCacheAsync(userId).ContinueWithOnFailedLog(_logger);
            }
            return result;
        }

        public async Task<UserInfo> GenerateItemAsync(Guid groupId, UserStatus status, MobilePassswordValidationCodeRegisterInput input, ModelStateDictionary modelState)
        {
            // 密码加密
            var password = UserService.EncryptPassword(input.Password);
            var userInfo = await _manager.GenerateItemAsync(groupId, status, input.Mobile, password, modelState);
            if (userInfo != null && userInfo.Status == UserStatus.Normal)
            {
                CacheUserAsync(userInfo).ContinueWithOnFailedLog(_logger);
            }
            return userInfo;
        }

        public async Task<bool> ResetPasswordAsync(MobileResetPassswordInput input, ModelStateDictionary modelState)
        {
            // 密码加密
            var password = UserService.EncryptPassword(input.Password);
            var userId = await _manager.ResetPasswordAsync(input.Mobile, password, modelState);
            if (userId <= 0 || !modelState.IsValid)
            {
                return false;
            }
            CleanCacheAsync(userId).ContinueWithOnFailedLog(_logger);
            return true;
        }

        public async Task<UserInfo> GetItemByMobileAsync(string mobile, bool mobileIsValid = true, UserStatus? status = null)
        {
            if (mobile.IsNullOrWhiteSpace()) return null;
            var userInfo = await _manager.GetItemByMobileAsync(mobile, mobileIsValid, status);
            if (userInfo != null && userInfo.Status == UserStatus.Normal)
            {
                CacheUserAsync(userInfo).ContinueWithOnFailedLog(_logger);
            }
            return userInfo;
        }

        public async Task<UserInfo> GetOrGenerateItemByMobileAsync(Guid groupId, UserStatus generateStatus, string mobile, bool mobileIsValid, ModelStateDictionary modelState)
        {
            var userInfo = await _manager.GetItemByMobileAsync(mobile);
            if (userInfo == null)
            {
                var password = UserService.GenerateRandomPassword(6);
                userInfo = await _manager.GenerateItemAsync(groupId, generateStatus, mobile, password, modelState);
                if (userInfo != null && userInfo.Status == UserStatus.Normal)
                {
                    CacheUserAsync(userInfo).ContinueWithOnFailedLog(_logger);
                }
            }
            return userInfo;
        }

        public async Task<bool> GetMobileValidationCodeAsync(GetMobileValidationCodeInput getMobileValidationCodeInput, ModelStateDictionary modelState)
        {
            if (getMobileValidationCodeInput.Type == MobileValidationCodeType.Register)
            {
                if (await _manager.IsExistsMobileAsync(getMobileValidationCodeInput.Mobile))
                {
                    modelState.AddModelError("Mobile", "手机号码已经被使用");
                    return false;
                }
            }
            else if (getMobileValidationCodeInput.Type == MobileValidationCodeType.Login || getMobileValidationCodeInput.Type == MobileValidationCodeType.ChangeMobile)
            {
                if (!await _manager.IsExistsMobileAsync(getMobileValidationCodeInput.Mobile))
                {
                    modelState.AddModelError("Mobile", "手机号码尚未注册");
                    return false;
                }
            }

            string validationCode = null;
            var cacheKey = MobileValidationCodeCacheKeyFormat.FormatWith(getMobileValidationCodeInput.Mobile);
            var mobileValidationCode = await _cache.GetJsonAsync<MobileValidationCode>(cacheKey);
            var now = DateTime.Now;
            if (mobileValidationCode != null)
            {
                if (now - mobileValidationCode.CreationTime < TimeSpan.FromSeconds(_mobileValidationCodeSettings.RequestInterval))
                {
                    modelState.AddModelError("Mobile", "请求过于频繁，请稍后再试");
                    return false;
                }

                if (!mobileValidationCode.ValidationCode.IsNullOrWhiteSpace() &&
                    mobileValidationCode.Type == getMobileValidationCodeInput.Type /* 验证码用途未发生更改 */ &&
                    mobileValidationCode.ExpirationDate <= now /* 验证码没到期 */ &&
                    mobileValidationCode.VerifyTimes < mobileValidationCode.MaxVerifyTimes /* 验证码在合理使用次数内 */)
                {
                    // 继续沿用之前的验证码
                    validationCode = mobileValidationCode.ValidationCode;
                }
            }

            if (validationCode == null)
            {
                validationCode = GenerateMobileValidationCode(_mobileValidationCodeSettings.CodeLength);
                mobileValidationCode = new MobileValidationCode
                {
                    Mobile = getMobileValidationCodeInput.Mobile,
                    Type = getMobileValidationCodeInput.Type,
                    ValidationCode = validationCode,
                    ExpirationDate = now.AddSeconds(_mobileValidationCodeSettings.Expiration),
                    MaxVerifyTimes = _mobileValidationCodeSettings.MaxVerifyTimes,
                    VerifyTimes = 0,
                    FinishVerifyDate = null,
                    CreationTime = now,
                };
                _cache.SetJsonAsync(cacheKey, mobileValidationCode, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromSeconds(_mobileValidationCodeSettings.Expiration)
                }).ContinueWithOnFailedLog(_logger);
            }
            var sms = "{\"code\":\"" + validationCode + "\",\"time\":\"" + (_mobileValidationCodeSettings.Expiration / 60) + "\"}";
            return await _smsSender.SendAsync(getMobileValidationCodeInput.Mobile, sms);
        }

        public async Task<bool> VerifyMobileValidationCodeAsync(VerifyMobileValidationCodeInput verifyMobileValidationCodeInput, ModelStateDictionary modelState, string defaultCode = null)
        {
            if (!defaultCode.IsNullOrWhiteSpace() && defaultCode.Equals(verifyMobileValidationCodeInput.ValidationCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            var cacheKey = MobileValidationCodeCacheKeyFormat.FormatWith(verifyMobileValidationCodeInput.Mobile);
            var mobileValidationCode = await _cache.GetJsonAsync<MobileValidationCode>(cacheKey);
            if (mobileValidationCode == null)
            {
                modelState.AddModelError("Mobile", "尚未请求验证码");
                return false;
            }

            mobileValidationCode.VerifyTimes++;
            // 可以接受的误差
            _cache.SetJsonAsync<MobileValidationCode>(cacheKey, mobileValidationCode, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(_mobileValidationCodeSettings.Expiration)
            }).ContinueWithOnFailedLog(_logger);

            if (mobileValidationCode.ValidationCode.IsNullOrWhiteSpace())
            {
                modelState.AddModelError("Mobile", "异常：尚未生成验证码");
                return false;
            }
            if (mobileValidationCode.Type != verifyMobileValidationCodeInput.Type)
            {
                modelState.AddModelError("Mobile", "手机验证码类型错误，请重新请求");
                return false;
            }
            if (mobileValidationCode.VerifyTimes > mobileValidationCode.MaxVerifyTimes)
            {
                modelState.AddModelError("Mobile", "手机验证码验证次数过多，请重新请求");
                return false;
            }
            if (DateTime.Now > mobileValidationCode.ExpirationDate)
            {
                modelState.AddModelError("Mobile", "手机验证码已经过期，请重新请求");
                return false;
            }
            if (mobileValidationCode.FinishVerifyDate != null)
            {
                modelState.AddModelError("Mobile", "手机验证码已经使用，请重新请求");
                return false;
            }
            if (!mobileValidationCode.ValidationCode.Equals(verifyMobileValidationCodeInput.ValidationCode, StringComparison.InvariantCultureIgnoreCase))
            {
                modelState.AddModelError("Mobile", "手机验证码输入错误，请重新输入");
                return false;
            }

            return true;
        }

        public async Task<bool> FinishVerifyMobileValidationCodeAsync(string mobile, MobileValidationCodeType type, ModelStateDictionary modelState)
        {
            var cacheKey = MobileValidationCodeCacheKeyFormat.FormatWith(mobile);
            var mobileValidationCode = await _cache.GetJsonAsync<MobileValidationCode>(cacheKey);
            if (mobileValidationCode == null || mobileValidationCode.ValidationCode.IsNullOrWhiteSpace())
            {
                modelState.AddModelError("Mobile", "尚未请求验证码");
                return false;
            }

            mobileValidationCode.FinishVerifyDate = DateTime.Now;
            _cache.SetJsonAsync(cacheKey, mobileValidationCode, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(_mobileValidationCodeSettings.Expiration)
            }).ContinueWithOnFailedLog(_logger);
            return true;
        }

        #endregion

        private Task CacheUserAsync(UserInfo userInfo)
        {
            var cacheKey = UserService.UserCacheKeyFormat.FormatWith(userInfo.UserId);
            return _cache.SetJsonAsync(cacheKey, userInfo, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(1)
            });
        }

        private Task CleanCacheAsync(int userId)
        {
            var cacheKey = UserService.UserCacheKeyFormat.FormatWith(userId);
            return _cache.RemoveAsync(cacheKey);
        }

        private string GenerateMobileValidationCode(int codeLength)
        {
            int[] randMembers = new int[codeLength];
            int[] validateNums = new int[codeLength];
            string validateNumberStr = String.Empty;
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = seekRand.Next(0, Int32.MaxValue - codeLength * 10000);
            int[] seeks = new int[codeLength];
            for (int i = 0; i < codeLength; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < codeLength; i++)
            {
                var rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, codeLength);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < codeLength; i++)
            {
                string numStr = randMembers[i].ToString(CultureInfo.InvariantCulture);
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < codeLength; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }
    }
}
