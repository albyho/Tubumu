using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

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
        public Guid RoleId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
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
        public bool IsSystem { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
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
        public virtual IEnumerable<PermissionBase> Permissions { get; set; }
    }
}
