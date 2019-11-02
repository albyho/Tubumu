using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tubumu.Modules.Admin.Sms;
using Tubumu.Sms;

namespace Tubumu.Modules.Admin.Application.Services
{
    public class SubMailSmsSender : ISmsSender
    {
        private readonly SubMailSmsSettings _subMailSmsSettings;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<SubMailSmsSender> _logger;

        public SubMailSmsSender(IOptions<SubMailSmsSettings> subMailSmsSettingsOptons, IHttpClientFactory clientFactory, ILogger<SubMailSmsSender> logger)
        {
            _clientFactory = clientFactory;
            _subMailSmsSettings = subMailSmsSettingsOptons.Value;
            _logger = logger;

    }
        public async Task<bool> SendAsync(SmsMessage smsMessage)
        {
            var client = _clientFactory.CreateClient();

            const string requestUri = "https://api.submail.cn/message/xsend.json";
            var httpContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("appid", _subMailSmsSettings.AppId),
                new KeyValuePair<string, string>("project", _subMailSmsSettings.Project),
                new KeyValuePair<string, string>("signature", _subMailSmsSettings.Signature),
                new KeyValuePair<string, string>("to", smsMessage.PhoneNumber),
                // message: "{\"code\":\""+ content +"\",\"time\":\""+ expirationInterval +"\"}"
                new KeyValuePair<string, string>("vars", smsMessage.Text),
            });
            try
            {
                var response = await client.PostAsync(requestUri, httpContent);
                var responseText = await response.Content.ReadAsStringAsync();
                // TODO: (alby)检查短信发送结果
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Submail");
                return false;
            }
        }
    }
}