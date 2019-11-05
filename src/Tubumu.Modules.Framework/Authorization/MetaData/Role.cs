using System;

namespace Tubumu.Modules.Framework.Authorization
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role
    {
        /// <summary>
        /// 角色 Id
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 权限 Id 集
        /// </summary>
        public Guid[] PermissionIds { get; set; }
    }
}
