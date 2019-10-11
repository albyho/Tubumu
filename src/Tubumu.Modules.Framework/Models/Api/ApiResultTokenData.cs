
using System.Text.Json.Serialization;

namespace Tubumu.Modules.Framework.Models
{
    /// <summary>
    /// ApiResult Token  Data
    /// </summary>
    public class ApiResultTokenData
    {
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// RefreshToken
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
