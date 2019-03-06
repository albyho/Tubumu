using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tubumu.Modules.Admin.Models
{
    /// <summary>
    /// 分组信息
    /// </summary>
    [Serializable]
    public class GroupInfo
    {
        /// <summary>
        /// 分组 Id
        /// </summary>
        [JsonProperty(PropertyName = "groupId")]
        public Guid GroupId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// 分组信息
    /// </summary>
    [Serializable]
    public class GroupBase
    {
        /// <summary>
        /// 分组 Id
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否允许包含用户
        /// </summary>
        public bool IsContainsUser { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// 是否是系统分组
        /// </summary>
        public bool IsSystem { get; set; }
    }

    /// <summary>
    /// 分组信息
    /// </summary>
    [Serializable]
    public class Group : GroupBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Group() {
            Roles = Enumerable.Empty<RoleBase>();
            AvailableRoles = Enumerable.Empty<RoleBase>();
            Permissions = Enumerable.Empty<PermissionBase>();
        }

        /// <summary>
        /// 父分组 Id
        /// </summary>
        public Guid? ParentId { set; get; }

        /// <summary>
        /// 层级（从 1 开始）
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 包含角色
        /// </summary>
        public virtual IEnumerable<RoleBase> Roles { get; set; }

        /// <summary>
        /// 分组可用的角色(该分组的用户的主要角色只能使用 AvailableRoles)
        /// </summary>
        public virtual IEnumerable<RoleBase> AvailableRoles { get; set; }

        /// <summary>
        /// 拥有权限
        /// </summary>
        public virtual IEnumerable<PermissionBase> Permissions { get; set; }
   }
}
