using System.Security.Claims;

namespace Tubumu.Modules.Framework.Authorization
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool HasPermission(this ClaimsPrincipal user, string permission)
        {
            if(user?.Identity.Name == null)
            {
                return false;
            }
            return user.HasClaim(TubumuClaimTypes.Permission, permission);
        }

        public static bool IsInGroup(this ClaimsPrincipal user, string group)
        {
            if(user?.Identity.Name == null)
            {
                return false;
            }
            return user.HasClaim(TubumuClaimTypes.Group, group);
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            if(user?.Identity.Name == null)
            {
                return -1;
            }
            if (int.TryParse(user.Identity.Name, out var userId))
            {
                return userId;
            }
            return -1;
        }
    }
}
