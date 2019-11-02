using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Tubumu.Sms
{
    /// <summary>
    /// NullSmsSender
    /// </summary>
    //[Dependency(TryRegister = true)]
    public class NullSmsSender : ISmsSender//, ISingletonDependency
    {
        /// <summary>
        /// Logger
        /// </summary>
        public ILogger<NullSmsSender> Logger { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public NullSmsSender()
        {
            Logger = NullLogger<NullSmsSender>.Instance;
        }

        /// <summary>
        /// SendAsync
        /// </summary>
        /// <param name="smsMessage"></param>
        /// <returns></returns>
        public Task<bool> SendAsync(SmsMessage smsMessage)
        {
            Logger.LogWarning($"SMS Sending was not implemented! Using {nameof(NullSmsSender)}:");

            Logger.LogWarning("Phone Number : " + smsMessage.PhoneNumber);
            Logger.LogWarning("SMS Text     : " + smsMessage.Text);

            return Task.FromResult(false);
        }
    }
}
