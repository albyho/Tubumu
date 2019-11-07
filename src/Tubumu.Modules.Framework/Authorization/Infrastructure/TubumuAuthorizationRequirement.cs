using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Tubumu.Core.Extensions;

namespace Tubumu.Modules.Framework.Authorization.Infrastructure
{
    public class TubumuAuthorizationRequirement : AuthorizationHandler<TubumuAuthorizationRequirement>, IAuthorizationRequirement
    {
        public TubumuAuthorizeData AuthorizeData { get; }

        public TubumuAuthorizationRequirement(TubumuAuthorizeData authorizeData)
        {
            AuthorizeData = authorizeData;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TubumuAuthorizationRequirement requirement)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            // 以半角逗号分隔的权限满足"需要"的其中之一即可，角色和分组也类似。
            // 分组、角色和权限三者在此也是 Or 的关系，所以是在尽力去找任一匹配。
            var found = false;
            if (requirement.AuthorizeData.Permissions != null)
            {
                var permissionsClaim = context.User.Claims.FirstOrDefault(c => string.Equals(c.Type, TubumuClaimTypes.Permission, StringComparison.OrdinalIgnoreCase));
                if (permissionsClaim?.Value != null && permissionsClaim.Value.Length > 0)
                {
                    var permissionsClaimSplit = SafeSplit(permissionsClaim.Value);
                    var permissionsDataSplit = SafeSplit(requirement.AuthorizeData.Permissions);
                    found = permissionsDataSplit.Intersect(permissionsClaimSplit).Any();
                }
            }

            if (!found && requirement.AuthorizeData.Roles != null)
            {
                var rolesClaim = context.User.Claims.FirstOrDefault(c => string.Equals(c.Type, ClaimTypes.Role, StringComparison.OrdinalIgnoreCase));
                if (rolesClaim?.Value != null && rolesClaim.Value.Length > 0)
                {
                    var rolesClaimSplit = SafeSplit(rolesClaim.Value);
                    var rolesDataSplit = SafeSplit(requirement.AuthorizeData.Roles);
                    found = rolesDataSplit.Intersect(rolesClaimSplit).Any();
                }
            }

            if (!found && requirement.AuthorizeData.Groups != null)
            {
                var groupsClaim = context.User.Claims.FirstOrDefault(c => string.Equals(c.Type, TubumuClaimTypes.Group, StringComparison.OrdinalIgnoreCase));
                if (groupsClaim?.Value != null && groupsClaim.Value.Length > 0)
                {
                    var groupsClaimSplit = SafeSplit(groupsClaim.Value);
                    var groupsDataSplit = SafeSplit(requirement.AuthorizeData.Groups);
                    found = groupsDataSplit.Intersect(groupsClaimSplit).Any();
                }
            }

            if (found)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private IEnumerable<string> SafeSplit(string source)
        {
            return source.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(m => m.Trim()).Where(m => !m.IsNullOrWhiteSpace());
        }
    }
}
