using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Admin.Domain.Services;
using Tubumu.Core.Extensions;
using Tubumu.Core.Utilities.Cryptography;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Application.Services
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
        /// 获取 AvatarUrl
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetAvatarUrlAsync(int userId);

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

        /// <summary>
        /// 验证除指定用户 Id 外，用户名是否被使用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<bool> VerifyExistsUsernameAsync(int userId, string username);

        /// <summary>
        /// 验证除指定用户 Id 外，邮箱是否被使用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<bool> VerifyExistsEmailAsync(int userId, string email);

        /// <summary>
        /// 验证除指定用户 Id 外，用户名、邮箱或手机是否被使用
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> VerifyExistsAsync(UserInput userInput, ModelStateDictionary modelState);

        /// <summary>
        /// 获取用户信息分页
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<Page<UserInfo>> GetPageAsync(UserPageSearchCriteria criteria);

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<UserInfo> SaveAsync(UserInput userInput, ModelStateDictionary modelState);

        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangeUsernameAsync(int userId, string username, ModelStateDictionary modelState);

        /// <summary>
        /// 修改显示名称（昵称）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="displayName"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangeDisplayNameAsync(int userId, string displayName, ModelStateDictionary modelState);

        /// <summary>
        /// 修改 AvatarUrl
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="avatarUrl"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangeAvatarAsync(int userId, string avatarUrl, ModelStateDictionary modelState);

        /// <summary>
        /// 修改 LogoUrl
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="logoUrl"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangeLogoAsync(int userId, string logoUrl, ModelStateDictionary modelState);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangePasswordAsync(int userId, string password, ModelStateDictionary modelState);

        /// <summary>
        /// 修改资料
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userChangeProfileInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangeProfileAsync(int userId, UserChangeProfileInput userChangeProfileInput, ModelStateDictionary modelState);

        /// <summary>
        /// 根据账号(用户名、邮箱或手机)重置密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ResetPasswordByAccountAsync(string account, string password, ModelStateDictionary modelState);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(int userId, ModelStateDictionary modelState);

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<bool> ChangeStatusAsync(int userId, UserStatus status);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="getUser"></param>
        /// <param name="afterSignIn"></param>
        /// <returns></returns>
        Task<bool> SignInAsync(Func<Task<UserInfo>> getUser, Action<UserInfo> afterSignIn = null);

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> SignOutAsync(int userId);
    }

    public class UserService : IUserService
    {
        /// <summary>
        /// 用户信息缓存 Key
        /// </summary>
        public const string UserCacheKeyFormat = "User:{0}";

        private readonly IUserManager _manager;
        private readonly IDistributedCache _cache;
        private readonly IGroupService _groupService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="manager"></param>
        /// <param name="groupService"></param>
        public UserService(
            IUserManager manager,
            IDistributedCache cache,
            IGroupService groupService
            )
        {
            _manager = manager;
            _cache = cache;
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
            var userInfo = await _manager.GetItemByUserIdAsync(userId, status);
            await CacheNormalUser(userInfo);
            return userInfo;
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
            var userInfo = await _manager.GetItemByUsernameAsync(username, status);
            await CacheNormalUser(userInfo);
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
            var userInfo = await _manager.GetItemByEmailAsync(email, emailIsValid, status);
            await CacheNormalUser(userInfo);
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
            var userInfo = await _manager.GetItemByMobileAsync(mobile, mobileIsValid, status);
            await CacheNormalUser(userInfo);
            return userInfo;
        }

        /// <summary>
        /// 获取用户信息列表
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public async Task<List<UserInfoWarpper>> GetUserInfoWarpperListAsync(IEnumerable<int> userIds)
        {
            return await _manager.GetUserInfoWarpperListAsync(userIds);
        }

        /// <summary>
        /// 获取 AvatarUrl
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetAvatarUrlAsync(int userId)
        {
            return await _manager.GetAvatarUrlAsync(userId);
        }

        /// <summary>
        /// 通过用户 Id 判断用户是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(int userId, UserStatus? status = null)
        {
            return await _manager.IsExistsAsync(userId, status);
        }

        /// <summary>
        /// 通过用户名判断用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsUsernameAsync(string username)
        {
            if (username.IsNullOrWhiteSpace()) return false;
            return await _manager.IsExistsUsernameAsync(username);
        }

        /// <summary>
        /// 通过邮箱判断用户是否存在
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsEmailAsync(string email)
        {
            if (email.IsNullOrWhiteSpace()) return false;
            return await _manager.IsExistsEmailAsync(email);
        }

        /// <summary>
        /// 验证除指定用户 Id 外，用户名是否被使用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> VerifyExistsUsernameAsync(int userId, string username)
        {
            if (username.IsNullOrWhiteSpace()) return false;
            return await _manager.VerifyExistsUsernameAsync(userId, username);
        }

        /// <summary>
        /// 验证除指定用户 Id 外，邮箱是否被使用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> VerifyExistsEmailAsync(int userId, string email)
        {
            if (email.IsNullOrWhiteSpace()) return false;
            return await _manager.VerifyExistsEmailAsync(userId, email);
        }

        /// <summary>
        /// 验证除指定用户 Id 外，用户名、邮箱或手机是否被使用
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> VerifyExistsAsync(UserInput userInput, ModelStateDictionary modelState)
        {
            return await _manager.VerifyExistsAsync(userInput, modelState);
        }

        /// <summary>
        /// 获取用户信息分页
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public async Task<Page<UserInfo>> GetPageAsync(UserPageSearchCriteria criteria)
        {
            await GengerateGroupIdsAsync(criteria);
            return await _manager.GetPageAsync(criteria);
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
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
            var userInfo = await _manager.SaveAsync(userInput, modelState);
            if (userInput is UserInputEdit userInputEdit)
            {
                await CleanCache(userInputEdit.UserId);
            }
            return userInfo;
        }

        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangeUsernameAsync(int userId, string username, ModelStateDictionary modelState)
        {
            bool result = await _manager.ChangeUsernameAsync(userId, username, modelState);
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

        /// <summary>
        /// 修改显示名称（昵称）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="displayName"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangeDisplayNameAsync(int userId, string displayName, ModelStateDictionary modelState)
        {
            bool result = await _manager.ChangeDisplayNameAsync(userId, displayName);
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

        /// <summary>
        /// 修改 AvatarUrl
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="avatarUrl"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangeAvatarAsync(int userId, string avatarUrl, ModelStateDictionary modelState)
        {
            var result = await _manager.ChangeAvatarAsync(userId, avatarUrl);
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

        /// <summary>
        /// 修改 LogoUrl
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="logoUrl"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangeLogoAsync(int userId, string logoUrl, ModelStateDictionary modelState)
        {
            bool result = await _manager.ChangeLogoAsync(userId, logoUrl);
            if (!result)
            {
                modelState.AddModelError("UserId", "修改 Logo 失败");
            }
            else
            {
                await CleanCache(userId);
            }
            return result;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangePasswordAsync(int userId, string password, ModelStateDictionary modelState)
        {
            var encryptedPassword = EncryptPassword(password);
            var result = await _manager.ChangePasswordAsync(userId, encryptedPassword, modelState);
            if (result)
            {
                await CleanCache(userId);
            }
            return result;
        }

        /// <summary>
        /// 修改资料
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userChangeProfileInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangeProfileAsync(int userId, UserChangeProfileInput userChangeProfileInput, ModelStateDictionary modelState)
        {
            bool result = await _manager.ChangeProfileAsync(userId, userChangeProfileInput);
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

        /// <summary>
        /// 根据账号(用户名、邮箱或手机)重置密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ResetPasswordByAccountAsync(string account, string password, ModelStateDictionary modelState)
        {
            var encryptedPassword = EncryptPassword(password);
            var userId = await _manager.ResetPasswordByAccountAsync(account, encryptedPassword, modelState);
            if (userId <= 0 || !modelState.IsValid)
            {
                return false;
            }

            await CleanCache(userId);
            return true;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(int userId, ModelStateDictionary modelState)
        {
            var result = await _manager.RemoveAsync(userId, modelState);
            if (result)
            {
                await CleanCache(userId);
            }
            return result;
        }

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<bool> ChangeStatusAsync(int userId, UserStatus status)
        {
            var result = await _manager.ChangeStatusAsync(userId, status);
            if (result)
            {
                await CleanCache(userId);
            }
            return result;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="getUser"></param>
        /// <param name="afterSignIn"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 用户注销
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
            return $"{passwordSalt}|{data}";
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

        private async Task CacheNormalUser(UserInfo userInfo)
        {
            if(userInfo == null || userInfo.Status != UserStatus.Normal) return;
            var cacheKey = UserCacheKeyFormat.FormatWith(userInfo.UserId);
            await _cache.SetJsonAsync(cacheKey, userInfo, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(1)
            });
        }

        private async Task CleanCache(int userId)
        {
            var cacheKey = UserCacheKeyFormat.FormatWith(userId);
            await _cache.RemoveAsync(cacheKey);
        }

        private async Task GengerateGroupIdsAsync(UserPageSearchCriteria criteria)
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
                userInfo = await _manager.GetItemByUserIdAsync(userId, UserStatus.Normal);
                await _cache.SetJsonAsync(cacheKey, userInfo, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromDays(1)
                });
            }
            return userInfo;
            /*
            if (!_cache.TryGetValue(cacheKey, out UserInfo userInfo))
            {
                // Key not in cache, so get data.
                userInfo = await _manager.GetItemByUserIdAsync(userId, UserStatus.Normal);
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
