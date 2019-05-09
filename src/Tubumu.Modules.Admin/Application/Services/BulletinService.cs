using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Admin.Domain.Services;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Framework.Extensions;

namespace Tubumu.Modules.Admin.Application.Services
{
    /// <summary>
    /// IBulletinService
    /// </summary>
    public interface IBulletinService
    {
        /// <summary>
        /// 从缓存获取
        /// </summary>
        /// <returns></returns>
        Task<Bulletin> GetItemInCacheAsync();

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="bulletin"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> SaveAsync(BulletinInput bulletin, ModelStateDictionary modelState);
    }

    /// <summary>
    /// BulletinService
    /// </summary>
    public class BulletinService : IBulletinService
    {
        private const string CacheKey = "Bulletin";

        private readonly IBulletinManager _manager;
        private readonly IDistributedCache _cache;
        private readonly ILogger<BulletinService> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        public BulletinService(IDistributedCache cache, IBulletinManager repository, ILogger<BulletinService> logger)
        {
            _cache = cache;
            _manager = repository;
            _logger = logger;
        }

        /// <summary>
        /// 从缓存获取
        /// </summary>
        /// <returns></returns>
        public Task<Bulletin> GetItemInCacheAsync()
        {
            return GetItemInCacheInternalAsync();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="bulletin"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> SaveAsync(BulletinInput bulletin, ModelStateDictionary modelState)
        {
            var result = await _manager.SaveAsync(bulletin, modelState);
            if (result)
            {
                _cache.RemoveAsync(CacheKey).ContinueWithOnFaultedLog(_logger);
            }
            return result;
        }

        private async Task<Bulletin> GetItemInCacheInternalAsync()
        {
            var bulletin = await _cache.GetJsonAsync<Bulletin>(CacheKey);
            if (bulletin == null)
            {
                bulletin = await _manager.GetItemAsync();
                _cache.SetJsonAsync(CacheKey, bulletin).ContinueWithOnFaultedLog(_logger);
            }
            return bulletin;

            /*
            if (!_cache.TryGetValue(CacheKey, out Bulletin bulletin))
            {
                // Key not in cache, so get data.
                bulletin = await _manager.GetItemAsync();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromDays(30));

                // Save data in cache.
                _cache.Set(CacheKey, bulletin, cacheEntryOptions);
            }

            return bulletin;
            */
        }
    }
}
