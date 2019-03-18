using System.Collections.Generic;

namespace Tubumu.Modules.Core.FastReflectionLib
{
    /// <summary>
    /// FastReflectionCache
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class FastReflectionCache<TKey, TValue> : IFastReflectionCache<TKey, TValue>
    {
        private Dictionary<TKey, TValue> m_cache = new Dictionary<TKey, TValue>();

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue Get(TKey key)
        {
            TValue value = default(TValue);
            if (this.m_cache.TryGetValue(key, out value))
            {
                return value;
            }

            lock (key)
            {
                if (!this.m_cache.TryGetValue(key, out value))
                {
                    value = this.Create(key);
                    this.m_cache[key] = value;
                }
            }

            return value;
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected abstract TValue Create(TKey key);
    }
}
