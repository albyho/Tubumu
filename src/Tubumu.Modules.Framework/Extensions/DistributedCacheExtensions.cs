using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Tubumu.Modules.Framework.Extensions.Object;

namespace Tubumu.Modules.Framework.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetJsonAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken)) where T : class
        {
            var json = value.ToJson();
            await distributedCache.SetStringAsync(key, json, options, token);
        }

        public static async Task SetJsonAsync<T>(this IDistributedCache distributedCache, string key, T value, CancellationToken token = default(CancellationToken)) where T : class
        {
            var json = value.ToJson();
            await distributedCache.SetStringAsync(key, json, token);
        }

        public static async Task<T> GetJsonAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken)) where T : class
        {
            var value = await distributedCache.GetStringAsync(key, token);
            return ObjectExtensions.FromJson<T>(value);
        }

        public static async Task SetObjectAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken)) where T : class
        {
            var bytes = value.ToByteArray();
            await distributedCache.SetAsync(key, bytes, options, token);
        }

        public static async Task SetObjectAsync<T>(this IDistributedCache distributedCache, string key, T value, CancellationToken token = default(CancellationToken)) where T : class
        {
            var bytes = value.ToByteArray();
            await distributedCache.SetAsync(key, bytes, token);
        }

        public static async Task<T> GetObjectAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken)) where T : class
        {
            var value = await distributedCache.GetAsync(key, token);
            return value.FromByteArray<T>();
        }
    }
}
