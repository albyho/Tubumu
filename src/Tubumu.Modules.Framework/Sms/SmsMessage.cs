using System.Collections.Generic;
using JetBrains.Annotations;
using Tubumu.Modules.Core;

namespace Tubumu.Modules.Framework.Sms
{
    /// <summary>
    /// SmsMessage
    /// </summary>
    public class SmsMessage
    {
        /// <summary>
        /// PhoneNumber
        /// </summary>
        public string PhoneNumber { get; }

        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Properties
        /// </summary>
        public IDictionary<string, object> Properties { get; }

        /// <summary>
        /// SmsMessage
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="text"></param>
        public SmsMessage(string phoneNumber, [NotNull] string text)
        {
            PhoneNumber = Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber));
            Text = Check.NotNullOrWhiteSpace(text, nameof(text));

            Properties = new Dictionary<string, object>();
        }
    }
}
