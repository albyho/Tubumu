﻿using System;
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

            // 当前用户的权限满足"需要"的其中之一即可，角色和分组也类似。
            // 分组、角色和权限三者在此也是 Or 的关系，所以是在尽力去找任一匹配。
            var found = false;
            if (requirement.AuthorizeData.Permissions != null)
            {
                var permissionsClaimSplit = context.User.Claims.Where(c => string.Equals(c.Type, TubumuClaimTypes.Permission, StringComparison.OrdinalIgnoreCase)).Select(m => m.Value);
                var permissionsDataSplit = SafeSplit(requirement.AuthorizeData.Permissions);
                found = permissionsDataSplit.Intersect(permissionsClaimSplit).Any();
            }

            if (!found && requirement.AuthorizeData.Roles != null)
            {
                var rolesClaimSplit = context.User.Claims.Where(c => string.Equals(c.Type, ClaimTypes.Role, StringComparison.OrdinalIgnoreCase)).Select(m => m.Value);
                var rolesDataSplit = SafeSplit(requirement.AuthorizeData.Roles);
                found = rolesDataSplit.Intersect(rolesClaimSplit).Any();
            }

            if (!found && requirement.AuthorizeData.Groups != null)
            {
                var groupsClaimSplit = context.User.Claims.Where(c => string.Equals(c.Type, TubumuClaimTypes.Group, StringComparison.OrdinalIgnoreCase)).Select(m => m.Value);
                var groupsDataSplit = SafeSplit(requirement.AuthorizeData.Groups);
                found = groupsDataSplit.Intersect(groupsClaimSplit).Any();
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
