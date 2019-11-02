using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;
using Tubumu.Sms;
using Tubumu.Swagger;

namespace Tubumu.Modules.Admin.Controllers
{
    /// <summary>
    /// Sms Controller (短信)
    /// </summary>
    [Route("Api/[controller]/[action]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [HiddenApi]
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
        [PermissionAuthorize(Permissions = "短信发送")]
        public async Task<ApiResult> Send(SendSmsInput input)
        {
            var returnResult = new ApiResult();
            var sendResult = await _smsSender.SendAsync(input.PhoneNumber, input.Text);
            if (!sendResult)
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
