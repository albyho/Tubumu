using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Tubumu.Modules.Core;

namespace Tubumu.Modules.Framework.Sms
{
    public static class SmsSenderExtensions
    {
        public static Task<bool> SendAsync([NotNull] this ISmsSender smsSender, [NotNull] string phoneNumber, [NotNull] string text)
        {
            Check.NotNull(smsSender, nameof(smsSender));
            return smsSender.SendAsync(new SmsMessage(phoneNumber, text));
        }
    }
}
