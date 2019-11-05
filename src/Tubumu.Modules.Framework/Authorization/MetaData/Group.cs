using System;

namespace Tubumu.Modules.Framework.Authorization
{
    /// <summary>
    /// 分组
    /// </summary>
    public class Group
    {
        /// <summary>
        /// 分组 Id
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// 父 Ids
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否允许包含用户
        /// </summary>
        public bool IsContainsUser { get; set; }

        /// <summary>
        /// 是否停用
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// 包含角色 Id 集
        /// </summary>
        public Guid[] RoleIds { get; set; }

        /// <summary>
        /// 分组可用的角色(该分组的用户的主要角色只能使用 AvailableRoles)
        /// </summary>
        public Guid[] AvailableRoleIds { get; set; }

        /// <summary>
        /// 拥有权限 Id 集
        /// </summary>
        public Guid[] PermissionIds { get; set; }
    }
}
