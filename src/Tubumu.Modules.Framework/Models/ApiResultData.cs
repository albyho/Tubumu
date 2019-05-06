using Newtonsoft.Json;

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
        [JsonProperty(PropertyName = "token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }

        /// <summary>
        /// RefreshToken
        /// </summary>
        [JsonProperty(PropertyName = "refreshToken", NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; }
    }
}
