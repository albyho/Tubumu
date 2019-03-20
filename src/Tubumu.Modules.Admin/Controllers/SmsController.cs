using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;
using Tubumu.Modules.Framework.Sms;

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
            var sendResult = await _smsSender.SendAsync(input.PhoneNumber, input.Text);
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
