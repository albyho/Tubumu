using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Services;

namespace Tubumu.Modules.Admin.Services
{
    /// <summary>
    /// ITokenService 扩展
    /// </summary>
    public static class TokenServiceExtensions
    {
        public static string GenerateAccessToken(this ITokenService tokenService,  UserInfo user)
        {
            var groups = from m in user.AllGroups select new Claim(TubumuClaimTypes.Group, m.Name);
            var roles = from m in user.AllRoles select new Claim(ClaimTypes.Role, m.Name);
            var permissions = from m in user.AllPermissions select new Claim(TubumuClaimTypes.Permission, m.Name);
            var claims = (new[] { new Claim(ClaimTypes.Name, user.UserId.ToString()) }).
                Union(groups).
                Union(roles).
                Union(permissions);
            return tokenService.GenerateAccessToken(claims);
        }

    }
}
