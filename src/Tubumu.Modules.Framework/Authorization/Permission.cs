using System;

namespace Tubumu.Modules.Framework.Authorization
{
    /// <summary>
    /// 权限
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// 权限 Ids
        /// </summary>
        public Guid PermissionId { get; set; }

        /// <summary>
        /// 父 Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Name { get; set; }
    }
}
