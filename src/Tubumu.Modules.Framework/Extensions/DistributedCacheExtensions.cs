using System.Threading;
using System.Threading.Tasks;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;
using Tubumu.Core.Extensions.Object;

namespace Tubumu.Modules.Framework.Extensions
{
    /// <summary>
    /// DistributedCacheExtensions
    /// </summary>
    public static class DistributedCacheExtensions
    {
        #region Json

        /// <summary>
        /// SetJsonAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Task SetJsonAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken)) where T : class
        {
            var json = value.ToJson();
            return distributedCache.SetStringAsync(key, json, options, token);
        }

        /// <summary>
        /// SetJsonAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Task SetJsonAsync<T>(this IDistributedCache distributedCache, string key, T value, CancellationToken token = default(CancellationToken)) where T : class
        {
            var json = value.ToJson();
            return distributedCache.SetStringAsync(key, json, token);
        }

        /// <summary>
        /// GetJsonAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<T> GetJsonAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken)) where T : class
        {
            var value = await distributedCache.GetStringAsync(key, token);
            return ObjectExtensions.FromJson<T>(value);
        }

        #endregion

        #region Object

        /// <summary>
        /// SetObjectAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Task SetObjectAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken)) where T : class
        {
            var bytes = value.ToByteArray();
            return distributedCache.SetAsync(key, bytes, options, token);
        }

        /// <summary>
        /// SetObjectAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Task SetObjectAsync<T>(this IDistributedCache distributedCache, string key, T value, CancellationToken token = default(CancellationToken)) where T : class
        {
            var bytes = value.ToByteArray();
            return distributedCache.SetAsync(key, bytes, token);
        }

        /// <summary>
        /// GetObjectAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<T> GetObjectAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken)) where T : class
        {
            var value = await distributedCache.GetAsync(key, token);
            if (value == null) return null;
            return value.FromByteArray<T>();
        }

        #endregion

        #region MessagePack

        /// <summary>
        /// SetPackAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Task SetPackAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken)) where T : class
        {
            var bytes = MessagePackSerializer.Serialize(value);
            return distributedCache.SetAsync(key, bytes, options, token);
        }

        /// <summary>
        /// SetPackAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Task SetPackAsync<T>(this IDistributedCache distributedCache, string key, T value, CancellationToken token = default(CancellationToken)) where T : class
        {
            var bytes = MessagePackSerializer.Serialize(value);
            return distributedCache.SetAsync(key, bytes, token);
        }

        /// <summary>
        /// GetPackAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<T> GetPackAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken)) where T : class
        {
            var value = await distributedCache.GetAsync(key, token);
            if (value == null) return null;
            return MessagePackSerializer.Deserialize<T>(value);
        }

        #endregion
    }
}
