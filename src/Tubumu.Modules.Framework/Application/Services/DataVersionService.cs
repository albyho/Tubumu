using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Framework.Application.Services
{
    public class DataVersionService : IDataVersionService
    {
        private const string RedisHashKey = "DataVersion";
        private readonly IRedisCacheClient _redis;
        private readonly IRedisDatabase _redisDatabase;
        private readonly ILogger<DataVersionService> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="redis"></param>
        /// <param name="cache"></param>
        /// <param name="logger"></param>
        public DataVersionService(IRedisCacheClient redis, ILogger<DataVersionService> logger)
        {
            _redis = redis;
            _redisDatabase = _redis.Db10;
            _logger = logger;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public async Task<bool> SetAsync(int typeId)
        {
            var cacheKey = DataVersion.CacheKeyWithTypeId(typeId);
            var dataVersion = await _redisDatabase.HashGetAsync<DataVersion>(RedisHashKey, cacheKey);
            if (dataVersion == null)
            {
                dataVersion = new DataVersion
                {
                    TypeId = typeId,
                    Version = 0,
                };
            }
            dataVersion.Version++;
            dataVersion.UpdateTime = DateTime.Now;
            CacheAsync(dataVersion).ContinueWithOnFaultedHandleLog(_logger);
            return true;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public async Task<DataVersion> GetAsync(int typeId)
        {
            var cacheKey = DataVersion.CacheKeyWithTypeId(typeId);
            var dataVersion = await _redisDatabase.HashGetAsync<DataVersion>(RedisHashKey, cacheKey);
            return dataVersion ?? new DataVersion
            {
                TypeId = typeId,
            };
        }

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DataVersion>> GetAllAsync()
        {
            var dataVersions = await _redisDatabase.HashGetAllAsync<DataVersion>(RedisHashKey);
            return dataVersions.Values;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public Task CleanupAsync(int typeId)
        {
            var cacheKey = DataVersion.CacheKeyWithTypeId(typeId);
            _redisDatabase.HashDeleteAsync(RedisHashKey, cacheKey).ContinueWithOnFaultedHandleLog(_logger);
            return Task.CompletedTask;
        }

        private Task CacheAsync(DataVersion dataVersion)
        {
            if (dataVersion == null)
            {
                throw new ArgumentNullException(nameof(dataVersion));
            }

            var cacheKey = DataVersion.CacheKeyWithTypeId(dataVersion.TypeId);
            _redisDatabase.HashSetAsync<DataVersion>(RedisHashKey, cacheKey, dataVersion).ContinueWithOnFaultedHandleLog(_logger);
            return Task.CompletedTask;
        }
    }
}
