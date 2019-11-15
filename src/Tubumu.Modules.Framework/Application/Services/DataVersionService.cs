using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;
using System.Linq;

namespace Tubumu.Modules.Framework.Application.Services
{
    public class DataVersionService : IDataVersionService
    {
        private const string RedisHashKey = "DataVersion";
        private const string GobalKeyPrefix = "Global";
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
        /// 更新数据。key 以 Gobal 作为前缀。
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public Task<bool> SetGlobalAsync(int typeId)
        {
            return SetAsync(GobalKeyPrefix, typeId);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="keyPrefix"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public async Task<bool> SetAsync(string keyPrefix, int typeId)
        {
            var key = $"{keyPrefix}:{typeId}";
            var dataVersion = await _redisDatabase.HashGetAsync<DataVersion>(RedisHashKey, key);
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

            _redisDatabase.HashSetAsync<DataVersion>(RedisHashKey, key, dataVersion).ContinueWithOnFaultedHandleLog(_logger);

            return true;
        }

        /// <summary>
        /// 获取数据。key 以 Global 作为前缀。
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public Task<DataVersion> GetGlobalAsync(int typeId)
        {
            return GetAsync(GobalKeyPrefix, typeId);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="keyPrefix"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public async Task<DataVersion> GetAsync(string keyPrefix, int typeId)
        {
            var key = $"{keyPrefix}:{typeId}";
            var dataVersion = await _redisDatabase.HashGetAsync<DataVersion>(RedisHashKey, key);
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
        /// 获取全部数据。key 以 Global 作为前缀。
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<DataVersion>> GetGlobalAllAsync()
        {
            return  GetAllAsync(GobalKeyPrefix);
        }

        /// <summary>
        /// 获取全部数据。仅获取 keyPrefix 作为前缀的 key 对应的数据。
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DataVersion>> GetAllAsync(string keyPrefix)
        {
            var dic = (await _redisDatabase.Database
                .HashGetAllAsync(RedisHashKey))
                .Where(m => m.Name.StartsWith(keyPrefix + ":"))
                .ToDictionary(
                    x => x.Name.ToString(),
                    x => _redisDatabase.Serializer.Deserialize<DataVersion>(x.Value),
                    StringComparer.Ordinal);
            return dic.Values;
        }

        /// <summary>
        /// 清除缓存。key 以 Global 作为前缀。
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public Task CleanupGlobalAsync(int typeId)
        {
            return CleanupAsync(GobalKeyPrefix, typeId);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="keyPrefix"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public Task CleanupAsync(string keyPrefix, int typeId)
        {
            var key = $"{keyPrefix}:{typeId}";
            _redisDatabase.HashDeleteAsync(RedisHashKey, key).ContinueWithOnFaultedHandleLog(_logger);
            return Task.CompletedTask;
        }
    }
}
