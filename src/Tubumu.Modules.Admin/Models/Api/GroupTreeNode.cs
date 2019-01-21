using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tubumu.Modules.Admin.Models.Api
{
    /// <summary>
    /// 用户分组树节点
    /// </summary>
    public class GroupTreeNode
    {
        /// <summary>
        /// 分组 Id
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
        public List<GroupTreeNode> Children { get; set; }

        /// <summary>
        /// 是否允许包含用户
        /// </summary>
        [JsonProperty(PropertyName = "isContainsUser")]
        public bool IsContainsUser { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [JsonProperty(PropertyName = "isDisabled")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// 是否是系统分组
        /// </summary>
        [JsonProperty(PropertyName = "isSystem")]
        public bool IsSystem { get; set; }

        /// <summary>
        /// 分组包含的角色
        /// </summary>
        [JsonProperty(PropertyName = "roles", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<RoleBase> Roles { get; set; }

        /// <summary>
        /// 分组可用的角色(该分组的用户的主要角色s只能使用 AvailableRoles)
        /// </summary>
        [JsonProperty(PropertyName = "availableRoles", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<RoleBase> AvailableRoles { get; set; }

        /// <summary>
        /// 分组拥有的权限
        /// </summary>
        [JsonProperty(PropertyName = "permissions", NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<PermissionBase> Permissions { get; set; }
    }
}
