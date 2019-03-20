using System.Reflection;

namespace Tubumu.Core.FastReflection
{
    /// <summary>
    /// PropertyAccessorFactory
    /// </summary>
    public class PropertyAccessorFactory : IFastReflectionFactory<PropertyInfo, IPropertyAccessor>
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IPropertyAccessor Create(PropertyInfo key)
        {
            return new PropertyAccessor(key);
        }

        #region IFastReflectionFactory<PropertyInfo,IPropertyAccessor> Members

        IPropertyAccessor IFastReflectionFactory<PropertyInfo, IPropertyAccessor>.Create(PropertyInfo key)
        {
            return Create(key);
        }

        #endregion
    }
}
