using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Swagger;

namespace Tubumu.Modules.Framework.Services
{
    /// <summary>
    /// Token Service
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// 生成 Access Token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        string GenerateAccessToken(IEnumerable<Claim> claims);

        /// <summary>
        /// 生成 Refresh Token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GenerateRefreshToken(int userId);

        /// <summary>
        /// 获取 Refresh Token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetRefreshToken(int userId);

        /// <summary>
        /// 废弃 Refresh Token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RevokeRefreshToken(int userId);

        /// <summary>
        /// 通过过期 Token 获取 ClaimsPrincipal
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
