using System.Reflection;

namespace Tubumu.Core.FastReflectionLib
{
    /// <summary>
    /// FieldAccessorCache
    /// </summary>
    public class FieldAccessorCache : FastReflectionCache<FieldInfo, IFieldAccessor>
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override IFieldAccessor Create(FieldInfo key)
        {
            return FastReflectionFactories.FieldAccessorFactory.Create(key);
        }
    }
}
