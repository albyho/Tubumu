using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubumu.Modules.Admin.Models.Api
{
    /// <summary>
    /// 权限树节点
    /// </summary>
    public class PermissionTreeNode
    {
        /// <summary>
        /// 权限 Id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        /// <summary>
        /// 父 Id
        /// </summary>
        [JsonProperty(PropertyName = "parentId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 父节点 Id 路径
        /// </summary>
        [JsonProperty(PropertyName = "parentIdPath", NullValueHandling = NullValueHandling.Ignore)]
        public List<Guid> ParentIdPath { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 层级(从 1 开始)
        /// </summary>
        [JsonProperty(PropertyName = "level")]
        public int Level { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [JsonProperty(PropertyName = "displayOrder")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        [JsonProperty(PropertyName = "children", NullValueHandling = NullValueHandling.Ignore)]
        public List<PermissionTreeNode> Children { get; set; }
    }
}
