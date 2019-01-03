using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Models.Api
{
    /// <summary>
    /// 区域树 ApiResult
    /// </summary>
    public class RegionTreeResult : ApiResult
    {
        /// <summary>
        /// 树
        /// </summary>
        [JsonProperty(PropertyName = "tree")]
        public List<RegionTreeNode> Tree { get; set; }
    }
}
