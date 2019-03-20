using System.Reflection;

namespace Tubumu.Core.FastReflection
{
    /// <summary>
    /// MethodInvokerFactory
    /// </summary>
    public class MethodInvokerFactory : IFastReflectionFactory<MethodInfo, IMethodInvoker>
    {
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IMethodInvoker Create(MethodInfo key)
        {
            return new MethodInvoker(key);
        }

        #region IFastReflectionFactory<MethodInfo,IMethodInvoker> Members

        IMethodInvoker IFastReflectionFactory<MethodInfo, IMethodInvoker>.Create(MethodInfo key)
        {
            return Create(key);
        }

        #endregion
    }
}
