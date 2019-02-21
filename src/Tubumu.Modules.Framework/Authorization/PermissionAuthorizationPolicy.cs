using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Tubumu.Modules.Framework.Authorization
{
    /// <summary>
    /// PermissionAuthorizationPolicy
    /// </summary>
    public class PermissionAuthorizationPolicy
    {
        /// <summary>
        /// Combines the <see cref="AuthorizationPolicy"/> provided by the specified
        /// <paramref name="policyProvider"/>.
        /// </summary>
        /// <param name="policyProvider">A <see cref="IAuthorizationPolicyProvider"/> which provides the policies to combine.</param>
        /// <param name="authorizeData">A collection of authorization data used to apply authorization to a resource.</param>
        /// <returns>
        /// A new <see cref="AuthorizationPolicy"/> which represents the combination of the
        /// authorization policies provided by the specified <paramref name="policyProvider"/>.
        /// </returns>
        public static async Task<AuthorizationPolicy> CombineAsync(IAuthorizationPolicyProvider policyProvider, IEnumerable<IAuthorizeData> authorizeData)
        {
            if (policyProvider == null)
            {
                throw new ArgumentNullException(nameof(policyProvider));
            }

            if (authorizeData == null)
            {
                throw new ArgumentNullException(nameof(authorizeData));
            }

            var policyBuilder = new AuthorizationPolicyBuilder();
            var any = false;
            foreach (var authorizeDatum in authorizeData)
            {
                any = true;
                var useDefaultPolicy = true;
                if (!string.IsNullOrWhiteSpace(authorizeDatum.Policy))
                {
                    var policy = await policyProvider.GetPolicyAsync(authorizeDatum.Policy);
                    if (policy == null)
                    {
                        //throw new InvalidOperationException(Resources.FormatException_AuthorizationPolicyNotFound(authorizeDatum.Policy));
                        throw new InvalidOperationException(nameof(authorizeDatum.Policy));
                    }
                    policyBuilder.Combine(policy);
                    useDefaultPolicy = false;
                }
                var rolesSplit = authorizeDatum.Roles?.Split(',');
                if (rolesSplit != null && rolesSplit.Any())
                {
                    var trimmedRolesSplit = rolesSplit.Where(r => !string.IsNullOrWhiteSpace(r)).Select(r => r.Trim());
                    policyBuilder.RequireRole(trimmedRolesSplit);
                    useDefaultPolicy = false;
                }
                if (authorizeDatum is IPermissionAuthorizeData permissionAuthorizeDatum)
                {
                    var groupsSplit = permissionAuthorizeDatum.Groups?.Split(',');
                    if (groupsSplit != null && groupsSplit.Any())
                    {
                        var trimmedGroupsSplit = groupsSplit.Where(r => !string.IsNullOrWhiteSpace(r)).Select(r => r.Trim());
                        policyBuilder.RequireClaim(TubumuClaimTypes.Group, trimmedGroupsSplit);
                        //policyBuilder.AddRequirements(new GroupsAuthorizationRequirement(trimmedGroupsSplit));
                        useDefaultPolicy = false;
                    }
                    var permissionsSplit = permissionAuthorizeDatum.Permissions?.Split(',');
                    if (permissionsSplit != null && permissionsSplit.Any())
                    {
                        var trimmedPermissionsSplit = permissionsSplit.Where(r => !string.IsNullOrWhiteSpace(r)).Select(r => r.Trim());
                        policyBuilder.RequireClaim(TubumuClaimTypes.Permission, trimmedPermissionsSplit);
                        //policyBuilder.AddRequirements(new PermissionsAuthorizationRequirement(trimmedPermissionsSplit));
                        useDefaultPolicy = false;
                    }
                }
                var authTypesSplit = authorizeDatum.AuthenticationSchemes?.Split(',');
                if (authTypesSplit != null && authTypesSplit.Any())
                {
                    foreach (var authType in authTypesSplit)
                    {
                        if (!string.IsNullOrWhiteSpace(authType))
                        {
                            policyBuilder.AuthenticationSchemes.Add(authType.Trim());
                        }
                    }
                }
                if (useDefaultPolicy)
                {
                    policyBuilder.Combine(await policyProvider.GetDefaultPolicyAsync());
                }
            }
            return any ? policyBuilder.Build() : null;
        }
    }
}
