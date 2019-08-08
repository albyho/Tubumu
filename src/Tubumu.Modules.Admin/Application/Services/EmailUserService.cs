using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Admin.Domain.Services;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Framework.Extensions;

namespace Tubumu.Modules.Admin.Application.Services
{
    public interface IEmailUserService
    {
        Task<bool> ChangeEmailAsync(int userId, string newEmail, bool emailIsValid, ModelStateDictionary modelState);

        Task<UserInfo> GetOrGenerateItemByEmailAsync(Guid groupId, UserStatus generateStatus, string email, bool emailIsValid, ModelStateDictionary modelState);
    }

    public class EmailUserService : IEmailUserService
    {
        private readonly IEmailUserManager _manager;
        private readonly IUserManager _userManager;
        private readonly IDistributedCache _cache;
        private readonly ILogger<EmailUserService> _logger;

        public EmailUserService(IEmailUserManager manager,
            IUserManager userManager,
            IDistributedCache cache,
            ILogger<EmailUserService> logger
            )
        {
            _manager = manager;
            _userManager = userManager;
            _cache = cache;
            _logger = logger;
        }

        #region IEmailUserService Members

        public async Task<bool> ChangeEmailAsync(int userId, string newEmail, bool emailIsValid, ModelStateDictionary modelState)
        {
            bool result = await _manager.ChangeEmailAsync(userId, newEmail, emailIsValid, modelState);
            if (!result)
            {
                modelState.AddModelError("UserId", "修改邮箱失败，可能当前用户不存在或新邮箱已经被使用");
            }
            else
            {
                CleanupUserCache(userId);
            }
            return result;
        }

        public async Task<UserInfo> GetOrGenerateItemByEmailAsync(Guid groupId, UserStatus generateStatus, string email, bool emailIsValid, ModelStateDictionary modelState)
        {
            var userInfo = await _userManager.GetItemByEmailAsync(email, null, null);
            if (userInfo == null)
            {
                var password = UserService.GenerateRandomPassword(6);
                userInfo = await _manager.GenerateItemAsync(groupId, generateStatus, email, password, modelState);
                if (userInfo != null && userInfo.Status == UserStatus.Normal)
                {
                    CacheUser(userInfo);
                }
            }
            return userInfo;
        }

        #endregion

        private void CacheUser(UserInfo userInfo)
        {
            var cacheKey = UserService.UserCacheKeyFormat.FormatWith(userInfo.UserId);
            _cache.SetJsonAsync(cacheKey, userInfo, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(1)
            }).ContinueWithOnFaultedHandleLog(_logger);
        }

        private void CleanupUserCache(int userId)
        {
            var cacheKey = UserService.UserCacheKeyFormat.FormatWith(userId);
            _cache.RemoveAsync(cacheKey).ContinueWithOnFaultedHandleLog(_logger);
        }
    }
}
