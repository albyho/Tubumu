using System.Threading.Tasks;

namespace Tubumu.Sms
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
