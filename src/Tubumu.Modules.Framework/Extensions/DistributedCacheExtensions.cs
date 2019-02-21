using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Tubumu.Modules.Framework.Extensions.Object;

namespace Tubumu.Modules.Framework.Extensions
{
    /// <summary>
    /// DistributedCacheExtensions
    /// </summary>
    public static class DistributedCacheExtensions
    {
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
        public static async Task SetJsonAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken)) where T : class
        {
            var json = value.ToJson();
            await distributedCache.SetStringAsync(key, json, options, token);
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
        public static async Task SetJsonAsync<T>(this IDistributedCache distributedCache, string key, T value, CancellationToken token = default(CancellationToken)) where T : class
        {
            var json = value.ToJson();
            await distributedCache.SetStringAsync(key, json, token);
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
        public static async Task SetObjectAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken)) where T : class
        {
            var bytes = value.ToByteArray();
            await distributedCache.SetAsync(key, bytes, options, token);
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
        public static async Task SetObjectAsync<T>(this IDistributedCache distributedCache, string key, T value, CancellationToken token = default(CancellationToken)) where T : class
        {
            var bytes = value.ToByteArray();
            await distributedCache.SetAsync(key, bytes, token);
        }

        /// <summary>
        /// GetObjectAsync
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distributedCache"></param>
        /// <param name="key"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<T> GetObjectAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken)) where T : class
        {
            var value = await distributedCache.GetAsync(key, token);
            return value.FromByteArray<T>();
        }
    }
}
