using System.Reflection;

namespace Tubumu.Modules.Core.FastReflectionLib
{
    /// <summary>
    /// PropertyAccessorCache
    /// </summary>
    public class PropertyAccessorCache : FastReflectionCache<PropertyInfo, IPropertyAccessor>
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected override IPropertyAccessor Create(PropertyInfo key)
        {
            return new PropertyAccessor(key);
        }
    }
}
