using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Admin.Repositories;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;
using Tubumu.Modules.Framework.Services;
using Tubumu.Modules.Framework.Utilities.Cryptography;

namespace Tubumu.Modules.Admin.Services
{
    public interface IUserService
    {
        /// <summary>
        /// 通过用户 Id 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<UserInfo> GetItemByUserIdAsync(int userId, UserStatus? status = null);

        /// <summary>
        /// 通过用户名获取用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<UserInfo> GetItemByUsernameAsync(string username, UserStatus? status = null);

        /// <summary>
        /// 通过邮箱获取用户信息
        /// </summary>
        /// <param name="email"></param>
        /// <param name="emailIsValid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<UserInfo> GetItemByEmailAsync(string email, bool emailIsValid = true, UserStatus? status = null);

        /// <summary>
        /// 通过手机号获取用户信息
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="mobileIsValid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<UserInfo> GetItemByMobileAsync(string mobile, bool mobileIsValid, UserStatus? status = null);

        /// <summary>
        /// 获取用户信息列表
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        Task<List<UserInfoWarpper>> GetUserInfoWarpperListAsync(IEnumerable<int> userIds);

        /// <summary>
        /// 获取 HeadUrl
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetHeadUrlAsync(int userId);

        /// <summary>
        /// 通过用户 Id 判断用户是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(int userId, UserStatus? status = null);

        /// <summary>
        /// 通过用户名判断用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<bool> IsExistsUsernameAsync(string username);

        /// <summary>
        /// 通过邮箱判断用户是否存在
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<bool> IsExistsEmailAsync(string email);
        Task<bool> VerifyExistsUsernameAsync(int userId, string username);
        Task<bool> VerifyExistsEmailAsync(int userId, string email);
        Task<bool> VerifyExistsAsync(UserInput userInput, ModelStateDictionary modelState);
        Task<Page<UserInfo>> GetPageAsync(UserSearchCriteria criteria);
        Task<UserInfo> SaveAsync(UserInput userInput, ModelStateDictionary modelState);
        Task<bool> ChangeUsernameAsync(int userId, string newUsername, ModelStateDictionary modelState);
        Task<bool> ChangeDisplayNameAsync(int userId, string newDisplayName, ModelStateDictionary modelState);
        Task<bool> ChangeLogoAsync(int userId, string logoUrl, ModelStateDictionary modelState);
        Task<bool> ChangePasswordAsync(int userId, string newPassword, ModelStateDictionary modelState);
        Task<bool> ChangeProfileAsync(int userId, UserChangeProfileInput userChangeProfileInput, ModelStateDictionary modelState);
        Task<bool> ResetPasswordByAccountAsync(string account, string newPassword, ModelStateDictionary modelState);
        Task<bool> ChangeHeadAsync(int userId, string newHead);
        Task<bool> RemoveAsync(int userId, ModelStateDictionary modelState);
        Task<bool> ChangeStatusAsync(int userId, UserStatus status);
        Task<bool> SignInAsync(Func<Task<UserInfo>> getUser, Action<UserInfo> afterSignIn = null);
        Task<bool> SignOutAsync(int userId);
    }

    public class UserService : IUserService
    {
        private readonly IDistributedCache _cache;
        private readonly IUserRepository _repository;
        private readonly IGroupService _groupService;

        /// <summary>
        /// 用户信息缓存 Key
        /// </summary>
        public const string UserCacheKeyFormat = "User:{0}";

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="repository"></param>
        /// <param name="groupService"></param>
        public UserService(
            IDistributedCache cache,
            IUserRepository repository,
            IGroupService groupService
            )
        {
            _cache = cache;
            _repository = repository;
            _groupService = groupService;
        }

        #region IUserService Members

        /// <summary>
        /// 通过用户 Id 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetItemByUserIdAsync(int userId, UserStatus? status = null)
        {
            if (status == UserStatus.Normal)
            {
                return await GetNormalItemByUserIdInCacheInternalAsync(userId);
            }
            return await _repository.GetItemByUserIdAsync(userId, status);
        }

        /// <summary>
        /// 通过用户名获取用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetItemByUsernameAsync(string username, UserStatus? status = null)
        {
            if (username.IsNullOrWhiteSpace()) return null;
            var userInfo = await _repository.GetItemByUsernameAsync(username, status);
            if (userInfo != null && userInfo.Status == UserStatus.Normal)
            {
                await CacheUser(userInfo);
            }
            return userInfo;
        }

        /// <summary>
        /// 通过邮箱获取用户信息
        /// </summary>
        /// <param name="email"></param>
        /// <param name="emailIsValid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetItemByEmailAsync(string email, bool emailIsValid = true, UserStatus? status = null)
        {
            if (email.IsNullOrWhiteSpace()) return null;
            var userInfo = await _repository.GetItemByEmailAsync(email, emailIsValid, status);
            if (userInfo != null && userInfo.Status == UserStatus.Normal)
            {
                await CacheUser(userInfo);
            }
            return userInfo;
        }

        /// <summary>
        /// 通过手机号获取用户信息
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="mobileIsValid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetItemByMobileAsync(string mobile, bool mobileIsValid = true, UserStatus? status = null)
        {
            if (mobile.IsNullOrWhiteSpace()) return null;
            var userInfo = await _repository.GetItemByMobileAsync(mobile, mobileIsValid, status);
            if (userInfo != null && userInfo.Status == UserStatus.Normal)
            {
                await CacheUser(userInfo);
            }
            return userInfo;
        }

        /// <summary>
        /// 获取用户信息列表
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public async Task<List<UserInfoWarpper>> GetUserInfoWarpperListAsync(IEnumerable<int> userIds)
        {
            return await _repository.GetUserInfoWarpperListAsync(userIds);
        }

        /// <summary>
        /// 获取 HeadUrl
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetHeadUrlAsync(int userId)
        {
            return await _repository.GetHeadUrlAsync(userId);
        }

        /// <summary>
        /// 通过用户 Id 判断用户是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(int userId, UserStatus? status = null)
        {
            return await _repository.IsExistsAsync(userId, status);
        }

        /// <summary>
        /// 通过用户名判断用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsUsernameAsync(string username)
        {
            if (username.IsNullOrWhiteSpace()) return false;
            return await _repository.IsExistsUsernameAsync(username);
        }

        /// <summary>
        /// 通过邮箱判断用户是否存在
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsEmailAsync(string email)
        {
            if (email.IsNullOrWhiteSpace()) return false;
            return await _repository.IsExistsEmailAsync(email);
        }
        public async Task<bool> VerifyExistsUsernameAsync(int userId, string username)
        {
            if (username.IsNullOrWhiteSpace()) return false;
            return await _repository.VerifyExistsUsernameAsync(userId, username);
        }
        public async Task<bool> VerifyExistsEmailAsync(int userId, string email)
        {
            if (email.IsNullOrWhiteSpace()) return false;
            return await _repository.VerifyExistsEmailAsync(userId, email);
        }
        public async Task<bool> VerifyExistsAsync(UserInput userInput, ModelStateDictionary modelState)
        {
            return await _repository.VerifyExistsAsync(userInput, modelState);
        }
        public async Task<Page<UserInfo>> GetPageAsync(UserSearchCriteria criteria)
        {
            await GengerateGroupIdsAsync(criteria);
            return await _repository.GetPageAsync(criteria);
        }
        public async Task<UserInfo> SaveAsync(UserInput userInput, ModelStateDictionary modelState)
        {
            //验证用户名、手机号码和邮箱是否被占用
            if (await VerifyExistsAsync(userInput, modelState))
            {
                return null;
            }
            //生成密码
            if (!userInput.Password.IsNullOrWhiteSpace())
                userInput.Password = userInput.PasswordConfirm = EncryptPassword(userInput.Password);
            else
                userInput.Password = userInput.PasswordConfirm = String.Empty;

            if (userInput.RealName.IsNullOrWhiteSpace())
            {
                userInput.RealNameIsValid = false;
            }
            //如果邮箱或手机为空，则验证也置为未通过
            if (userInput.Email.IsNullOrWhiteSpace())
            {
                userInput.EmailIsValid = false;
            }
            if (userInput.Mobile.IsNullOrWhiteSpace())
            {
                userInput.MobileIsValid = false;
            }
            //保存实体
            var userInfo = await _repository.SaveAsync(userInput, modelState);
            if (userInfo != null && userInfo.Status == UserStatus.Normal)
            {
                await CacheUser(userInfo);
            }
            return userInfo;
        }
        public async Task<bool> ChangeUsernameAsync(int userId, string newUsername, ModelStateDictionary modelState)
        {
            bool result = await _repository.ChangeUsernameAsync(userId, newUsername, modelState);
            if (!result)
            {
                modelState.AddModelError("UserId", "修改用户名失败，可能当前用户不存在或新用户名已经被使用");
            }
            else
            {
                await CleanCache(userId);
            }
            return result;
        }
        public async Task<bool> ChangeDisplayNameAsync(int userId, string newDisplayName, ModelStateDictionary modelState)
        {
            bool result = await _repository.ChangeDisplayNameAsync(userId, newDisplayName);
            if (!result)
            {
                modelState.AddModelError("UserId", "修改昵称失败");
            }
            else
            {
                await CleanCache(userId);
            }
            return result;
        }
        public async Task<bool> ChangeLogoAsync(int userId, string logoUrl, ModelStateDictionary modelState)
        {
            bool result = await _repository.ChangeLogoAsync(userId, logoUrl);
            if (!result)
            {
                modelState.AddModelError("UserId", "修改头像失败");
            }
            else
            {
                await CleanCache(userId);
            }
            return result;
        }
        public async Task<bool> ChangePasswordAsync(int userId, string newPassword, ModelStateDictionary modelState)
        {
            var password = EncryptPassword(newPassword);
            var result = await _repository.ChangePasswordAsync(userId, password, modelState);
            if (result)
            {
                await CleanCache(userId);
            }
            return result;
        }
        public async Task<bool> ChangeProfileAsync(int userId, UserChangeProfileInput userChangeProfileInput, ModelStateDictionary modelState)
        {
            bool result = await _repository.ChangeProfileAsync(userId, userChangeProfileInput);
            if (!result)
            {
                modelState.AddModelError("UserId", "修改资料失败，可能当前用户不存在");
            }
            else
            {
                await CleanCache(userId);
            }
            return result;

        }
        public async Task<bool> ResetPasswordByAccountAsync(string account, string newPassword, ModelStateDictionary modelState)
        {
            var password = EncryptPassword(newPassword);
            var userId = await _repository.ResetPasswordByAccountAsync(account, password, modelState);
            if (userId <= 0 || !modelState.IsValid)
            {
                return false;
            }

            await CleanCache(userId);
            return true;
        }
        public async Task<bool> ChangeHeadAsync(int userId, string newHead)
        {
            var result = await _repository.ChangeHeadAsync(userId, newHead);
            if (result)
            {
                await CleanCache(userId);
            }
            return result;
        }
        public async Task<bool> RemoveAsync(int userId, ModelStateDictionary modelState)
        {
            var result = await _repository.RemoveAsync(userId, modelState);
            if (result)
            {
                await CleanCache(userId);
            }
            return result;
        }
        public async Task<bool> ChangeStatusAsync(int userId, UserStatus status)
        {
            var result = await _repository.ChangeStatusAsync(userId, status);
            if (result)
            {
                await CleanCache(userId);
            }
            return result;
        }
        public async Task<bool> SignInAsync(Func<Task<UserInfo>> getUser, Action<UserInfo> afterSignIn = null)
        {
            var user = await getUser();
            if (user != null)
            {
                await CleanCache(user.UserId);
                return true;
            }

            return false;
        }
        public async Task<bool> SignOutAsync(int userId)
        {
            await CleanCache(userId);
            return true;
        }

        #endregion

        /// <summary>
        /// 加密密码
        /// </summary>
        /// <param name="rawPassword"></param>
        /// <returns></returns>
        public static string EncryptPassword(string rawPassword)
        {
            if (rawPassword.IsNullOrWhiteSpace()) return String.Empty;
            string passwordSalt = Guid.NewGuid().ToString("N");
            string data = SHA256.Encrypt(rawPassword, passwordSalt);
            return "{0}|{1}".FormatWith(passwordSalt, data);
        }

        /// <summary>
        /// 生成随机密码
        /// </summary>
        /// <param name="pwdlen"></param>
        /// <returns></returns>
        public static string GenerateRandomPassword(int pwdlen)
        {
            const string pwdChars = "abcdefghijklmnopqrstuvwxyz0123456789";
            string tmpstr = "";
            var rnd = new Random();
            for (int i = 0; i < pwdlen; i++)
            {
                int iRandNum = rnd.Next(pwdChars.Length);
                tmpstr += pwdChars[iRandNum];
            }
            return tmpstr;
        }

        private async Task CacheUser(UserInfo userInfo)
        {
            var cacheKey = UserCacheKeyFormat.FormatWith(userInfo.UserId);
            await _cache.SetJsonAsync<UserInfo>(cacheKey, userInfo, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(1)
            });
        }

        private async Task CleanCache(int userId)
        {
            var cacheKey = UserCacheKeyFormat.FormatWith(userId);
            await _cache.RemoveAsync(cacheKey);
        }

        private async Task GengerateGroupIdsAsync(UserSearchCriteria criteria)
        {
            if (!criteria.GroupIds.IsNullOrEmpty())
            {
                var newGroupIds = new List<Guid>();
                foreach (var groupId in criteria.GroupIds)
                {
                    var groupIds = (await _groupService.GetListInCacheAsync(groupId)).Select(m => m.GroupId);
                    newGroupIds.AddRange(groupIds);
                }
                criteria.GroupIds = newGroupIds;
            }
        }

        private async Task<UserInfo> GetNormalItemByUserIdInCacheInternalAsync(int userId)
        {
            var cacheKey = UserCacheKeyFormat.FormatWith(userId);
            var userInfo = await _cache.GetJsonAsync<UserInfo>(cacheKey);
            if (userInfo == null)
            {
                userInfo = await _repository.GetItemByUserIdAsync(userId, UserStatus.Normal);
                await _cache.SetJsonAsync<UserInfo>(cacheKey, userInfo, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromDays(1)
                });
            }
            return userInfo;
            /*
            if (!_cache.TryGetValue(cacheKey, out UserInfo userInfo))
            {
                // Key not in cache, so get data.
                userInfo = await _repository.GetItemByUserIdAsync(userId, UserStatus.Normal);
                if(userInfo == null) return null;

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromDays(30));

                // Save data in cache.
                _cache.Set(cacheKey, userInfo, cacheEntryOptions);
            }

            return userInfo;
            */
        }
    }
}
