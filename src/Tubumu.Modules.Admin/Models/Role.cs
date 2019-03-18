using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Tubumu.Modules.Admin.Models
{
    /// <summary>
    /// 角色信息
    /// </summary>
    [Serializable]
    public class RoleInfo
    {
        /// <summary>
        /// 角色 Id
        /// </summary>
        [JsonProperty(PropertyName = "roleId")]
        public Guid RoleId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// 角色信息
    /// </summary>
    [Serializable]
    public class RoleBase : RoleInfo
    {
        /// <summary>
        /// 是否是系统角色
        /// </summary>
        [JsonProperty(PropertyName = "isSystem")]
        public bool IsSystem { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [JsonProperty(PropertyName = "displayOrder")]
        public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// 角色信息
    /// </summary>
    [Serializable]
    public class Role : RoleBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Role()
        {
            Permissions = Enumerable.Empty<PermissionBase>();
        }

        /// <summary>
        /// 拥有权限
        /// </summary>
        [JsonProperty(PropertyName = "permissions", NullValueHandling = NullValueHandling.Ignore)]
        public virtual IEnumerable<PermissionBase> Permissions { get; set; }
    }
}
