using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Tubumu.Modules.Admin.Models
{
    /// <summary>
    /// 权限信息
    /// </summary>
    [Serializable]
    public class PermissionBase
    {
        /// <summary>
        /// 权限 Id
        /// </summary>
        public Guid PermissionId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 权限信息
    /// </summary>
    [Serializable]
    public class Permission : PermissionBase
    {
        /// <summary>
        /// 所属权限
        /// </summary>
        [DisplayName("所属权限")]
        public Guid? ParentId { set; get; }

        /// <summary>
        /// 层级
        /// </summary>
        [DisplayName("层级")]
        public int Level { set; get; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [DisplayName("显示顺序")]
        public int DisplayOrder { set; get; }
    }
}
