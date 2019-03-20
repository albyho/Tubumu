using System.Threading.Tasks;

namespace Tubumu.Modules.Framework.Sms
{
    /// <summary>
    /// ISmsSender
    /// </summary>
    public interface ISmsSender
    {
        /// <summary>
        /// SendAsync
        /// </summary>
        /// <param name="smsMessage"></param>
        /// <returns></returns>
        Task<bool> SendAsync(SmsMessage smsMessage);
    }
}
