using Newtonsoft.Json;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Models
{
    /// <summary>
    /// ApiResult Token Mobile Data
    /// </summary>
    public class ApiResultTokenWithMobileData : ApiResultTokenData
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }
    }
}
