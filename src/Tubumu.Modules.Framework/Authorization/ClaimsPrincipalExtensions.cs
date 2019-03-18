using System.Security.Claims;

namespace Tubumu.Modules.Framework.Authorization
{
    /// <summary>
    /// ClaimsPrincipal Extensions
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// 用户是否拥有权限
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public static bool HasPermission(this ClaimsPrincipal user, string permission)
        {
            if (user?.Identity.Name == null)
            {
                return false;
            }
            return user.HasClaim(TubumuClaimTypes.Permission, permission);
        }

        /// <summary>
        /// 用户是否在分组
        /// </summary>
        /// <param name="user"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public static bool IsInGroup(this ClaimsPrincipal user, string group)
        {
            if (user?.Identity.Name == null)
            {
                return false;
            }
            return user.HasClaim(TubumuClaimTypes.Group, group);
        }

        /// <summary>
        /// 获取用户 Id
        /// </summary>
        /// <remarks>Name 保存用户 Id</remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int GetUserId(this ClaimsPrincipal user)
        {
            if (user?.Identity.Name == null)
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
