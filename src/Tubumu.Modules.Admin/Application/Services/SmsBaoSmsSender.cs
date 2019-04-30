using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tubumu.Modules.Admin.Sms;
using Tubumu.Modules.Framework.Sms;

namespace Tubumu.Modules.Admin.Application.Services
{
    public class SmsBaoSmsSender : ISmsSender
    {
        private readonly SmsBaoSmsSettings _smsBaoSmsSettings;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<SmsBaoSmsSender> _logger;

        public SmsBaoSmsSender(IOptions<SmsBaoSmsSettings> smsBaoSmsSettingsOptions, IHttpClientFactory clientFactory, ILogger<SmsBaoSmsSender> logger)
        {
            _clientFactory = clientFactory;
            _smsBaoSmsSettings = smsBaoSmsSettingsOptions.Value;
            _logger = logger;
        }

        public async Task<bool> SendAsync(SmsMessage smsMessage)
        {
            var client = _clientFactory.CreateClient();
            const string requestUriFormat = "https://api.smsbao.com/sms?u={0}&p={1}&m={2}&c={3}";
            // 如果需要 %20 转 + , 则用 Uri.EscapeDataString(someString);
            var urlEncodedMessage = WebUtility.UrlEncode(smsMessage.Text);
            var requestUri = String.Format(requestUriFormat, _smsBaoSmsSettings.Username, _smsBaoSmsSettings.Password, smsMessage.PhoneNumber, urlEncodedMessage);
            try
            {
                var sendResult = await client.GetStringAsync(requestUri);
                var result = sendResult == "0";
                if (!result)
                {
                    _logger.LogError("SmsBaoSmsSender 发送短信失败：手机号：{0} 内容：{1} 错误号：{2} 错误消息：{3}", smsMessage.PhoneNumber, smsMessage.Text, sendResult, GetErrorMesssage(sendResult));
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string GetErrorMesssage(string errorCode)
        {
            /*
            30	错误密码
            40	账号不存在
            41	余额不足
            43	IP地址限制
            50	内容含有敏感词
            51	手机号码不正确
            */
            switch (errorCode)
            {
                case "30":
                    {
                        return "错误密码";
                    }
                case "40":
                    {
                        return "账号不存在";
                    }
                case "41":
                    {
                        return "余额不足";
                    }
                case "43":
                    {
                        return "IP地址限制";
                    }
                case "50":
                    {
                        return "内容含有敏感词";
                    }
                case "51":
                    {
                        return "手机号码不正确";
                    }
                default:
                    {
                        return "未知错误";
                    }
            }
        }
    }
}