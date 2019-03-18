using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Tubumu.Modules.Framework.Authorization.Infrastructure
{
    /// <summary>
    /// Implements an <see cref="IAuthorizationHandler"/> and <see cref="IAuthorizationRequirement"/>
    /// which requires at least one role claim whose value must be any of the allowed permissions.
    /// </summary>
    public class PermissionsAuthorizationRequirement : AuthorizationHandler<PermissionsAuthorizationRequirement>, IAuthorizationRequirement
    {
        private const string ClaimType = "Permissions";
        /// <summary>
        /// Creates a new instance of <see cref="PermissionsAuthorizationRequirement"/>.
        /// </summary>
        /// <param name="allowedPermissions">A collection of allowed permisssions.</param>
        public PermissionsAuthorizationRequirement(IEnumerable<string> allowedPermissions)
        {
            if (allowedPermissions == null)
            {
                throw new ArgumentNullException(nameof(allowedPermissions));
            }

            if (!allowedPermissions.Any())
            {
                //throw new InvalidOperationException(Resources.Exception_RoleRequirementEmpty);
                throw new InvalidOperationException(nameof(allowedPermissions));
            }
            AllowedPermissions = allowedPermissions;
        }

        /// <summary>
        /// Gets the collection of allowed roles.
        /// </summary>
        public IEnumerable<string> AllowedPermissions { get; }

        /// <summary>
        /// Makes a decision if authorization is allowed based on a specific requirement.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                var found = false;
                if (requirement.AllowedPermissions == null || !requirement.AllowedPermissions.Any())
                {
                    // Review: What do we want to do here?  No roles requested is auto success?
                }
                else
                {
                    var permissionsClaim = context.User.Claims.FirstOrDefault(c => string.Equals(c.Type, ClaimType, StringComparison.OrdinalIgnoreCase));
                    if (permissionsClaim?.Value != null && permissionsClaim.Value.Length > 0)
                    {
                        var permissionsClaimSplit = permissionsClaim.Value.Split(',');
                        found = requirement.AllowedPermissions.Intersect(permissionsClaimSplit).Any();
                    }
                }
                if (found)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }

    }
}
