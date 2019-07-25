using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Framework.Application.Services;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Application.Services
{
    /// <summary>
    /// ITokenService 扩展
    /// </summary>
    public static class TokenServiceExtensions
    {
        /// <summary>
        /// GenerateAccessToken
        /// </summary>
        /// <param name="tokenService"></param>
        /// <param name="userInfo"></param>
        /// <param name="extendedClaims"></param>
        /// <returns></returns>
        public static string GenerateAccessToken(this ITokenService tokenService, UserInfo userInfo, IEnumerable<Claim> extendedClaims = null)
        {
            var groups = from m in userInfo.AllGroups select new Claim(TubumuClaimTypes.Group, m.Name);
            var roles = from m in userInfo.AllRoles select new Claim(ClaimTypes.Role, m.Name);
            var permissions = from m in userInfo.AllPermissions select new Claim(TubumuClaimTypes.Permission, m.Name);
            var claims = (new[] { new Claim(ClaimTypes.Name, userInfo.UserId.ToString()) }).
                Union(groups).
                Union(roles).
                Union(permissions);
            if (extendedClaims != null)
            {
                claims = claims.Union(extendedClaims);
            }
            return tokenService.GenerateAccessToken(claims);
        }

        /// <summary>
        /// GenerateApiResultTokenData
        /// </summary>
        /// <param name="tokenService"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static async Task<ApiResultTokenData> GenerateApiResultTokenData(this ITokenService tokenService, UserInfo userInfo)
        {
            var token = tokenService.GenerateAccessToken(userInfo);
            var refreshToken = await tokenService.GenerateRefreshTokenAsync(userInfo.UserId);
            return new ApiResultTokenData
            {
                Token = token,
                RefreshToken = refreshToken,
            };
        }

        /// <summary>
        /// ApiResultTokenWithMobileData
        /// </summary>
        /// <param name="tokenService"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public static async Task<ApiResultTokenWithMobileData> GenerateApiResultTokenWithMobileData(this ITokenService tokenService, UserInfo userInfo)
        {
            var token = tokenService.GenerateAccessToken(userInfo);
            var refreshToken = await tokenService.GenerateRefreshTokenAsync(userInfo.UserId);
            return new ApiResultTokenWithMobileData
            {
                Token = token,
                RefreshToken = refreshToken,
                Mobile = userInfo.Mobile,
            };
        }

    }
}
