using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Senparc.Weixin.WxOpen.AdvancedAPIs.Sns;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Admin.Domain.Services;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Weixin;
using Tubumu.Modules.Framework.Extensions;

namespace Tubumu.Modules.Admin.Application.Services
{
    /// <summary>
    /// IWeixinUserService
    /// </summary>
    public interface IWeixinUserService
    {
        /// <summary>
        /// GetItemByWeixinMobileEndOpenIdAsync
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        Task<UserInfo> GetItemByWeixinMobileEndOpenIdAsync(string openId);

        /// <summary>
        /// GetItemByWeixinAppOpenIdAsync
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        Task<UserInfo> GetItemByWeixinAppOpenIdAsync(string openId);

        /// <summary>
        /// GetItemByWeixinWebOpenIdAsync
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        Task<UserInfo> GetItemByWeixinWebOpenIdAsync(string openId);

        /// <summary>
        /// GetItemByWeixinUnionIdAsync
        /// </summary>
        /// <param name="unionId"></param>
        /// <returns></returns>
        Task<UserInfo> GetItemByWeixinUnionIdAsync(string unionId);

        /// <summary>
        /// GetWeixinAppOpenIdAsync
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<string> GetWeixinAppOpenIdAsync(string code);

        /// <summary>
        /// GetWeixinMobileEndOpenIdAsync
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<string> GetWeixinMobileEndOpenIdAsync(string code);

        /// <summary>
        /// GetWeixinWebOpenIdAsync
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<string> GetWeixinWebOpenIdAsync(string code);

        /// <summary>
        /// GetOrGenerateItemByWeixinMobileEndOpenIdAsync
        /// </summary>
        /// <param name="generateGroupId"></param>
        /// <param name="generateStatus"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        Task<UserInfo> GetOrGenerateItemByWeixinMobileEndOpenIdAsync(Guid generateGroupId, UserStatus generateStatus, string openId);

        /// <summary>
        /// GetOrGenerateItemByWeixinAppOpenIdAsync
        /// </summary>
        /// <param name="generateGroupId"></param>
        /// <param name="generateStatus"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        Task<UserInfo> GetOrGenerateItemByWeixinAppOpenIdAsync(Guid generateGroupId, UserStatus generateStatus, string openId);

        /// <summary>
        /// GetOrGenerateItemByWeixinWebOpenIdAsync
        /// </summary>
        /// <param name="generateGroupId"></param>
        /// <param name="generateStatus"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        Task<UserInfo> GetOrGenerateItemByWeixinWebOpenIdAsync(Guid generateGroupId, UserStatus generateStatus, string openId);

        /// <summary>
        /// GetOrGenerateItemByWeixinUnionIdAsync
        /// </summary>
        /// <param name="generateGroupId"></param>
        /// <param name="generateStatus"></param>
        /// <param name="unionId"></param>
        /// <returns></returns>
        Task<UserInfo> GetOrGenerateItemByWeixinUnionIdAsync(Guid generateGroupId, UserStatus generateStatus, string unionId);

        /// <summary>
        /// UpdateWeixinMobileEndOpenIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="openId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> UpdateWeixinMobileEndOpenIdAsync(int userId, string openId, ModelStateDictionary modelState);

        /// <summary>
        /// CleanWeixinMobileEndOpenIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> CleanWeixinMobileEndOpenIdAsync(int userId);

        /// <summary>
        /// UpdateWeixinAppOpenIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="openId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> UpdateWeixinAppOpenIdAsync(int userId, string openId, ModelStateDictionary modelState);

        /// <summary>
        /// CleanWeixinAppOpenIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> CleanWeixinAppOpenIdAsync(int userId);

        /// <summary>
        /// UpdateWeixinWebOpenIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="openId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> UpdateWeixinWebOpenIdAsync(int userId, string openId, ModelStateDictionary modelState);

        /// <summary>
        /// CleanWeixinWebOpenIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> CleanWeixinWebOpenIdAsync(int userId);

        /// <summary>
        /// UpdateWeixinUnionIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="unionId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> UpdateWeixinUnionIdAsync(int userId, string unionId, ModelStateDictionary modelState);

        /// <summary>
        /// CleanWeixinUnionIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> CleanWeixinUnionIdAsync(int userId);

    }

    /// <summary>
    /// WeixinUserService
    /// </summary>
    public class WeixinUserService : IWeixinUserService
    {
        private readonly IDistributedCache _cache;
        private readonly IWeixinUserManager _manager;
        private readonly WeixinAppSettings _weixinAppSettings;

        /// <summary>
        /// Constructorss
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="manager"></param>
        /// <param name="weixinAppSettingsOptions"></param>
        public WeixinUserService(IDistributedCache cache,
            IWeixinUserManager manager,
            IOptions<WeixinAppSettings> weixinAppSettingsOptions
            )
        {
            _manager = manager;
            _cache = cache;
            _weixinAppSettings = weixinAppSettingsOptions.Value;
        }

        #region IWeixinUserService Members

        /// <summary>
        /// GetItemByWeixinMobileEndOpenIdAsync
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetItemByWeixinMobileEndOpenIdAsync(string openId)
        {
            var userInfo = await _manager.GetItemByWeixinMobileEndOpenIdAsync(openId);
            if (userInfo != null && userInfo.Status == UserStatus.Normal)
            {
                await CacheUser(userInfo);
            }
            return userInfo;
        }

        /// <summary>
        /// GetItemByWeixinAppOpenIdAsync
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetItemByWeixinAppOpenIdAsync(string openId)
        {
            var userInfo = await _manager.GetItemByWeixinAppOpenIdAsync(openId);
            if (userInfo != null && userInfo.Status == UserStatus.Normal)
            {
                await CacheUser(userInfo);
            }
            return userInfo;
        }

        /// <summary>
        /// GetItemByWeixinWebOpenIdAsync
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetItemByWeixinWebOpenIdAsync(string openId)
        {
            var userInfo = await _manager.GetItemByWeixinWebOpenIdAsync(openId);
            if (userInfo != null && userInfo.Status == UserStatus.Normal)
            {
                await CacheUser(userInfo);
            }
            return userInfo;
        }

        /// <summary>
        /// GetItemByWeixinUnionIdAsync
        /// </summary>
        /// <param name="unionId"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetItemByWeixinUnionIdAsync(string unionId)
        {
            var userInfo = await _manager.GetItemByWeixinUnionIdAsync(unionId);
            if (userInfo != null && userInfo.Status == UserStatus.Normal)
            {
                await CacheUser(userInfo);
            }
            return userInfo;
        }

        /// <summary>
        /// GetWeixinAppOpenIdAsync
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<string> GetWeixinAppOpenIdAsync(string code)
        {
            // https://developers.weixin.qq.com/miniprogram/dev/api/code2Session.html
            // GET https://api.weixin.qq.com/sns/oauth2/access_token?appid=APPID&secret=SECRET&code=CODE&grant_type=authorization_code
            try
            {
                var jsCode2JsonResult = await SnsApi.JsCode2JsonAsync(_weixinAppSettings.AppId, _weixinAppSettings.Secret, code);
                return jsCode2JsonResult.openid;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// GetWeixinMobileEndOpenIdAsync
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<string> GetWeixinMobileEndOpenIdAsync(string code)
        {
            // https://open.weixin.qq.com/cgi-bin/showdocument?action=dir_list&t=resource/res_list&verify=1&id=open1419317851&token=&lang=zh_CN
            // GET https://api.weixin.qq.com/sns/oauth2/access_token?appid=APPID&secret=SECRET&code=CODE&grant_type=authorization_code
            try
            {
                var jsCode2JsonResult = await SnsApi.JsCode2JsonAsync(_weixinAppSettings.AppId, _weixinAppSettings.Secret, code);
                return jsCode2JsonResult.openid;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// GetWeixinWebOpenIdAsync
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<string> GetWeixinWebOpenIdAsync(string code)
        {
            // https://open.weixin.qq.com/cgi-bin/showdocument?action=dir_list&t=resource/res_list&verify=1&id=open1419316505&token=&lang=zh_CN
            // GET https://api.weixin.qq.com/sns/jscode2session?appid=APPID&secret=SECRET&js_code=JSCODE&grant_type=authorization_code
            try
            {
                var jsCode2JsonResult = await SnsApi.JsCode2JsonAsync(_weixinAppSettings.AppId, _weixinAppSettings.Secret, code);
                return jsCode2JsonResult.openid;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// GetOrGenerateItemByWeixinMobileEndOpenIdAsync
        /// </summary>
        /// <param name="generateGroupId"></param>
        /// <param name="generateStatus"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetOrGenerateItemByWeixinMobileEndOpenIdAsync(Guid generateGroupId, UserStatus generateStatus, string openId)
        {
            var userInfo = await _manager.GetOrGenerateItemByWeixinMobileEndOpenIdAsync(generateGroupId, generateStatus, openId);
            if (userInfo != null && userInfo.Status == UserStatus.Normal)
            {
                await CacheUser(userInfo);
            }
            return userInfo;
        }

        /// <summary>
        /// GetOrGenerateItemByWeixinAppOpenIdAsync
        /// </summary>
        /// <param name="generateGroupId"></param>
        /// <param name="generateStatus"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetOrGenerateItemByWeixinAppOpenIdAsync(Guid generateGroupId, UserStatus generateStatus, string openId)
        {
            var userInfo = await _manager.GetOrGenerateItemByWeixinAppOpenIdAsync(generateGroupId, generateStatus, openId);
            if (userInfo != null && userInfo.Status == UserStatus.Normal)
            {
                await CacheUser(userInfo);
            }
            return userInfo;
        }

        /// <summary>
        /// GetOrGenerateItemByWeixinWebOpenIdAsync
        /// </summary>
        /// <param name="generateGroupId"></param>
        /// <param name="generateStatus"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetOrGenerateItemByWeixinWebOpenIdAsync(Guid generateGroupId, UserStatus generateStatus, string openId)
        {
            var userInfo = await _manager.GetOrGenerateItemByWeixinWebOpenIdAsync(generateGroupId, generateStatus, openId);
            if (userInfo != null && userInfo.Status == UserStatus.Normal)
            {
                await CacheUser(userInfo);
            }
            return userInfo;
        }

        /// <summary>
        /// GetOrGenerateItemByWeixinUnionIdAsync
        /// </summary>
        /// <param name="generateGroupId"></param>
        /// <param name="generateStatus"></param>
        /// <param name="unionId"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetOrGenerateItemByWeixinUnionIdAsync(Guid generateGroupId, UserStatus generateStatus, string unionId)
        {
            var userInfo = await _manager.GetOrGenerateItemByWeixinUnionIdAsync(generateGroupId, generateStatus, unionId);
            if (userInfo != null && userInfo.Status == UserStatus.Normal)
            {
                await CacheUser(userInfo);
            }
            return userInfo;
        }

        /// <summary>
        /// UpdateWeixinMobileEndOpenIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="openId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> UpdateWeixinMobileEndOpenIdAsync(int userId, string openId, ModelStateDictionary modelState)
        {
            var result = await _manager.UpdateWeixinMobileEndOpenIdAsync(userId, openId, modelState);
            if (result)
            {
                await CleanCache(userId);
            }
            return result;
        }

        /// <summary>
        /// CleanWeixinMobileEndOpenIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> CleanWeixinMobileEndOpenIdAsync(int userId)
        {
            var result = await _manager.CleanWeixinMobileEndOpenIdAsync(userId);
            if (result)
            {
                await CleanCache(userId);
            }
            return result;
        }

        /// <summary>
        /// UpdateWeixinAppOpenIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="openId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> UpdateWeixinAppOpenIdAsync(int userId, string openId, ModelStateDictionary modelState)
        {
            var result = await _manager.UpdateWeixinAppOpenIdAsync(userId, openId, modelState);
            if (result)
            {
                await CleanCache(userId);
            }
            return result;
        }

        /// <summary>
        /// CleanWeixinAppOpenIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> CleanWeixinAppOpenIdAsync(int userId)
        {
            var result = await _manager.CleanWeixinAppOpenIdAsync(userId);
            if (result)
            {
                await CleanCache(userId);
            }
            return result;
        }

        /// <summary>
        /// UpdateWeixinWebOpenIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="openId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> UpdateWeixinWebOpenIdAsync(int userId, string openId, ModelStateDictionary modelState)
        {
            var result = await _manager.UpdateWeixinWebOpenIdAsync(userId, openId, modelState);
            if (result)
            {
                await CleanCache(userId);
            }
            return result;
        }

        /// <summary>
        /// CleanWeixinWebOpenIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> CleanWeixinWebOpenIdAsync(int userId)
        {
            var result = await _manager.CleanWeixinWebOpenIdAsync(userId);
            if (result)
            {
                await CleanCache(userId);
            }
            return result;
        }

        /// <summary>
        /// UpdateWeixinUnionIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="openId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> UpdateWeixinUnionIdAsync(int userId, string openId, ModelStateDictionary modelState)
        {
            var result = await _manager.UpdateWeixinUnionIdAsync(userId, openId, modelState);
            if (result)
            {
                await CleanCache(userId);
            }
            return result;
        }

        /// <summary>
        /// CleanWeixinUnionIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> CleanWeixinUnionIdAsync(int userId)
        {
            var result = await _manager.CleanWeixinUnionIdAsync(userId);
            if (result)
            {
                await CleanCache(userId);
            }
            return result;
        }

        #endregion

        private async Task CacheUser(UserInfo userInfo)
        {
            var cacheKey = UserService.UserCacheKeyFormat.FormatWith(userInfo.UserId);
            await _cache.SetJsonAsync(cacheKey, userInfo, new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(1)
            });
        }

        private async Task CleanCache(int userId)
        {
            var cacheKey = UserService.UserCacheKeyFormat.FormatWith(userId);
            await _cache.RemoveAsync(cacheKey);
        }
    }
}
