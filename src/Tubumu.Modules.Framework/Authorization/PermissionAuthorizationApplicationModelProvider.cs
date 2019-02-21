using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Tubumu.Modules.Framework.Authorization
{
    /// <summary>
    /// PermissionAuthorizationApplicationModelProvider
    /// </summary>
    public class PermissionAuthorizationApplicationModelProvider : IApplicationModelProvider
    {
        private readonly IAuthorizationPolicyProvider _policyProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="policyProvider"></param>
        public PermissionAuthorizationApplicationModelProvider(IAuthorizationPolicyProvider policyProvider)
        {
            _policyProvider = policyProvider;
        }

        /// <summary>
        /// 序号
        /// </summary>
        public int Order => -1000 + 10;

        /// <summary>
        /// OnProvidersExecuted
        /// </summary>
        /// <param name="context"></param>
        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            // Intentionally empty.
        }

        /// <summary>
        /// OnProvidersExecuting
        /// </summary>
        /// <param name="context"></param>
        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            foreach (var controllerModel in context.Result.Controllers)
            {
                var controllerModelAuthData = controllerModel.Attributes.OfType<IAuthorizeData>().ToArray();
                if (controllerModelAuthData.Length > 0)
                {
                    controllerModel.Filters.Add(GetFilter(_policyProvider, controllerModelAuthData));
                }
                foreach (var _ in controllerModel.Attributes.OfType<IAllowAnonymous>())
                {
                    controllerModel.Filters.Add(new AllowAnonymousFilter());
                }

                foreach (var actionModel in controllerModel.Actions)
                {
                    var actionModelAuthData = actionModel.Attributes.OfType<IAuthorizeData>().ToArray();
                    if (actionModelAuthData.Length > 0)
                    {
                        actionModel.Filters.Add(GetFilter(_policyProvider, actionModelAuthData));
                    }

                    foreach (var _ in actionModel.Attributes.OfType<IAllowAnonymous>())
                    {
                        actionModel.Filters.Add(new AllowAnonymousFilter());
                    }
                }
            }
        }

        /// <summary>
        /// GetFilter
        /// </summary>
        /// <param name="policyProvider"></param>
        /// <param name="authData"></param>
        /// <returns></returns>
        public static AuthorizeFilter GetFilter(IAuthorizationPolicyProvider policyProvider, IEnumerable<IAuthorizeData> authData)
        {
            // The default policy provider will make the same policy for given input, so make it only once.
            // This will always execute synchronously.
            if (policyProvider.GetType() == typeof(DefaultAuthorizationPolicyProvider))
            {
                var policy = PermissionAuthorizationPolicy.CombineAsync(policyProvider, authData).GetAwaiter().GetResult();
                return new AuthorizeFilter(policy);
            }
            else
            {
                return new AuthorizeFilter(policyProvider, authData);
            }
        }
    }
}
