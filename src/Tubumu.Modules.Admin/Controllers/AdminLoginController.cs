using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tubumu.Core.Utilities.Security;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Admin.Services;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Controllers
{
    /// <summary>
    /// 后台：登录注销
    /// </summary>
    public partial class AdminController
    {
        #region  Login

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetValidationCode")]
        [AllowAnonymous]
        public ActionResult GetValidationCode()
        {
            var vCode = new ValidationCodeCreater(4, out string code);
            HttpContext.Session.SetString(ValidationCodeKey, code);
            byte[] bytes = vCode.CreateValidationCodeGraphic();
            return File(bytes, @"image/png");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<ApiResultDataUrl<ApiResultTokenData>> Login([FromBody]AccountPasswordValidationCodeLoginInput input)
        {
            var result = new ApiResultDataUrl<ApiResultTokenData>();
            var validationCode = HttpContext.Session.GetString(ValidationCodeKey);
            if (validationCode == null)
            {
                result.Code = 400;
                result.Message = "验证码已到期，请重新输入";
                return result;
            }

            if (String.Compare(validationCode, input.ValidationCode, StringComparison.OrdinalIgnoreCase) != 0)
            {
                result.Code = 400;
                result.Message = "请输入正确的验证码";
                return result;
            }

            HttpContext.Session.Remove(ValidationCodeKey);

            var user = await _userService.GetNormalUserAsync(input.Account, input.Password);
            if (user == null)
            {
                result.Code = 400;
                result.Message = "账号或密码错误，或用户状态不允许登录";
                return result;
            }

            var token = _tokenService.GenerateAccessToken(user);
            var refreshToken = await _tokenService.GenerateRefreshToken(user.UserId);
            result.Data = new ApiResultTokenData
            {
                Token = token,
                RefreshToken = refreshToken,
            };
            result.Url = _frontendSettings.CoreEnvironment.IsDevelopment ? _frontendSettings.CoreEnvironment.DevelopmentHost + "/modules/index.html" : Url.Action("Index", "View");
            result.Code = 200;
            result.Message = "登录成功";
            return result;
        }

        #endregion

        #region  Logout

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [HttpPost("Logout")]
        [AllowAnonymous]
        public async Task<ApiResultUrl> Logout()
        {
            var userId = HttpContext.User.GetUserId();
            if (userId >= 0)
            {
                await _tokenService.RevokeRefreshToken(userId);
                await _userService.SignOutAsync(userId);
            }
            var result = new ApiResultUrl
            {
                Code = 200,
                Message = "注销成功",
                Url = _frontendSettings.CoreEnvironment.IsDevelopment ? _frontendSettings.CoreEnvironment.DevelopmentHost + "/modules/login.html" : Url.Action("Login", "View"),
            };

            return result;
        }

        #endregion

        #region RefreshToken

        /// <summary>
        /// 刷新 Token
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RefreshToken")]
        [AllowAnonymous]
        public async Task<ApiResultDataUrl<ApiResultTokenData>> RefreshToken([FromBody]RefreshTokenInput input)
        {
            var result = new ApiResultDataUrl<ApiResultTokenData>();
            var principal = _tokenService.GetPrincipalFromExpiredToken(input.Token);
            var userId = principal.GetUserId(); //this is mapped to the Name claim by default

            var storeRefreshToken = await _tokenService.GetRefreshToken(userId);
            if (storeRefreshToken != input.RefreshToken)
            {
                result.Code = 200;
                result.Message = "刷新 Token 失败";
                result.Url = _frontendSettings.CoreEnvironment.IsDevelopment ? _frontendSettings.CoreEnvironment.DevelopmentHost + "/modules/login.html" : Url.Action("Login", "View");
                return result;
            }

            var newToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = await _tokenService.GenerateRefreshToken(userId);
            result.Data = new ApiResultTokenData
            {
                Token = newToken,
                RefreshToken = newRefreshToken,
            };
            result.Code = 200;
            result.Message = "刷新 Token 成功";
            return result;
        }

        #endregion
    }
}
