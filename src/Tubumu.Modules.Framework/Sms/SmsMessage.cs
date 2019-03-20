using System.Collections.Generic;
using JetBrains.Annotations;
using Tubumu.Modules.Core;

namespace Tubumu.Modules.Framework.Sms
{
    public class SmsMessage
    {
        public string PhoneNumber { get; }

        public string Text { get; }

        public IDictionary<string, object> Properties { get; }

        public SmsMessage(string phoneNumber, [NotNull] string text)
        {
            PhoneNumber = Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber));
            Text = Check.NotNullOrWhiteSpace(text, nameof(text));

            Properties = new Dictionary<string, object>();
        }
    }
}
