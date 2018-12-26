﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using Tubumu.Modules.Admin.Entities;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Api;
using Tubumu.Modules.Admin.Models.InputModels;
using Tubumu.Modules.Admin.Services;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;
using Tubumu.Modules.Framework.Swagger;
using Tubumu.Modules.Framework.Utilities.Security;
using Group = Tubumu.Modules.Admin.Models.Group;
using Permission = Tubumu.Modules.Admin.Models.Permission;
using SignatureHelper = Tubumu.Modules.Framework.Authorization.SignatureHelper;

namespace Tubumu.Modules.Admin.Controllers
{
    public partial class AdminController : ControllerBase
    {

        #region  Login

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

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiTokenResult>> Login([FromBody]AccountPasswordValidationCodeInput input)
        {
            var result = new ApiTokenResult();
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

            var groups = from m in user.AllGroups select new Claim(TubumuClaimTypes.Group, m.Name);
            var roles = from m in user.AllRoles select new Claim(ClaimTypes.Role, m.Name);
            var permissions = from m in user.AllPermissions select new Claim(TubumuClaimTypes.Permission, m.Name);
            var claims = (new[] { new Claim(ClaimTypes.Name, user.UserId.ToString()) }).
                Union(groups).
                Union(roles).
                Union(permissions);
            var token = new JwtSecurityToken(
                _tokenValidationSettings.ValidIssuer,
                _tokenValidationSettings.ValidAudience,
                claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: SignatureHelper.GenerateSigningCredentials(_tokenValidationSettings.IssuerSigningKey));

            var jwt = _tokenHandler.WriteToken(token);

            result.Token = jwt;
            result.Url = _frontendSettings.CoreEnvironment.IsDevelopment ? _frontendSettings.CoreEnvironment.DevelopmentHost + "/modules/index.html" : Url.Action("Index", "View");
            result.Code = 200;
            result.Message = "登录成功";
            return result;
        }

        #endregion

        #region  Logout

        [HttpPost("Logout")]
        public ApiResult Logout()
        {
            _userService.SignOutAsync();
            var result = new ApiResult
            {
                Code = 200,
                Message = "注销成功",
                Url = _frontendSettings.CoreEnvironment.IsDevelopment ? _frontendSettings.CoreEnvironment.DevelopmentHost + "/modules/login.html" : Url.Action("Login", "View"),
            };

            return result;
        }

        #endregion
    }
}