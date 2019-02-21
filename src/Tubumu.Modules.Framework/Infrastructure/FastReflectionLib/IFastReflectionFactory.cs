using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tubumu.Modules.Framework.Infrastructure.FastReflectionLib
{
    /// <summary>
    /// IFastReflectionFactory
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IFastReflectionFactory<TKey, TValue>
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TValue Create(TKey key);
    }
}
