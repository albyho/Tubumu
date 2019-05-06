using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="repository"></param>
        public BulletinService(IDistributedCache cache, IBulletinManager repository)
        {
            _cache = cache;
            _manager = repository;
        }

        /// <summary>
        /// 从缓存获取
        /// </summary>
        /// <returns></returns>
        public async Task<Bulletin> GetItemInCacheAsync()
        {
            return await GetItemInCacheInternalAsync();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="bulletin"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> SaveAsync(BulletinInput bulletin, ModelStateDictionary modelState)
        {
            bool result = await _manager.SaveAsync(bulletin, modelState);
            if (result)
            {
                await _cache.RemoveAsync(CacheKey);
            }
            return result;
        }

        private async Task<Bulletin> GetItemInCacheInternalAsync()
        {
            var bulletin = await _cache.GetJsonAsync<Bulletin>(CacheKey);
            if (bulletin == null)
            {
                bulletin = await _manager.GetItemAsync();
                await _cache.SetJsonAsync(CacheKey, bulletin);
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
