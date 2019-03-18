using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Tubumu.Modules.Framework.Authorization.Infrastructure
{
    /// <summary>
    /// Implements an <see cref="IAuthorizationHandler"/> and <see cref="IAuthorizationRequirement"/>
    /// which requires at least one role claim whose value must be any of the allowed groups.
    /// </summary>
    public class GroupsAuthorizationRequirement : AuthorizationHandler<GroupsAuthorizationRequirement>, IAuthorizationRequirement
    {
        private const string ClaimType = "Groups";
        /// <summary>
        /// Creates a new instance of <see cref="GroupsAuthorizationRequirement"/>.
        /// </summary>
        /// <param name="allowedGrous">A collection of allowed groups.</param>
        public GroupsAuthorizationRequirement(IEnumerable<string> allowedGrous)
        {
            if (allowedGrous == null)
            {
                throw new ArgumentNullException(nameof(allowedGrous));
            }

            if (!allowedGrous.Any())
            {
                //throw new InvalidOperationException(Resources.Exception_RoleRequirementEmpty);
                throw new InvalidOperationException(nameof(allowedGrous));
            }
            AllowedGrous = allowedGrous;
        }

        /// <summary>
        /// Gets the collection of allowed roles.
        /// </summary>
        public IEnumerable<string> AllowedGrous { get; }

        /// <summary>
        /// Makes a decision if authorization is allowed based on a specific requirement.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The requirement to evaluate.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupsAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                var found = false;
                if (requirement.AllowedGrous == null || !requirement.AllowedGrous.Any())
                {
                    // Review: What do we want to do here?  No roles requested is auto success?
                }
                else
                {
                    var groupsClaim = context.User.Claims.FirstOrDefault(c => string.Equals(c.Type, ClaimType, StringComparison.OrdinalIgnoreCase));
                    if (groupsClaim?.Value != null && groupsClaim.Value.Length > 0)
                    {
                        var groupsClaimSplit = groupsClaim.Value.Split(',');
                        found = requirement.AllowedGrous.Intersect(groupsClaimSplit).Any();
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
