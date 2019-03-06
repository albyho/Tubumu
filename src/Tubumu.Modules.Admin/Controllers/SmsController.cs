using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Tubumu.Modules.Admin.Frontend;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Admin.ModuleMenus;
using Tubumu.Modules.Admin.Services;
using Tubumu.Modules.Admin.Settings;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Models;
using Tubumu.Modules.Framework.Swagger;
using Senparc.Weixin.Open;
using Tubumu.Modules.Framework.Services;

namespace Tubumu.Modules.Admin.Controllers
{
    /// <summary>
    /// Sms Controller
    /// </summary>
    /// <remarks>短信</remarks>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class SmsController : ControllerBase
    {
        private readonly ISmsSender _smsSender;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="smsSender"></param>
        public SmsController(
            ISmsSender smsSender
            )
        {
            _smsSender = smsSender;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult> Send(SendSmsInput input)
        {
            var returnResult = new ApiResult();
            var sendResult = await _smsSender.SendAsync(input.Mobile, input.Content);
            if(!sendResult)
            {
                returnResult.Code = 400;
                returnResult.Message = ModelState.FirstErrorMessage();
                return returnResult;
            }
            returnResult.Code = 200;
            returnResult.Message = "发送成功";
            return returnResult;
        }
    }
}
