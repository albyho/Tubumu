using System.Threading.Tasks;
using JetBrains.Annotations;
using Tubumu.Modules.Core;

namespace Tubumu.Sms
{
    /// <summary>
    /// SmsSender extensions
    /// </summary>
    public static class SmsSenderExtensions
    {
        /// <summary>
        /// SendAsync
        /// </summary>
        /// <param name="smsSender"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Task<bool> SendAsync([NotNull] this ISmsSender smsSender, [NotNull] string phoneNumber, [NotNull] string text)
        {
            Check.NotNull(smsSender, nameof(smsSender));
            return smsSender.SendAsync(new SmsMessage(phoneNumber, text));
        }
    }
}
