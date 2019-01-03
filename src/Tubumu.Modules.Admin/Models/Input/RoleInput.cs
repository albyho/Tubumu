using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 角色 Id Input
    /// </summary>
    public class RoleIdInput
    {
        [Required(ErrorMessage = "请输入角色 Id")]
        public Guid RoleId { get; set; }
    }

    /// <summary>
    /// 角色 Input
    /// </summary>
    public class RoleInput
    {
        /// <summary>
        /// 角色 Id
        /// </summary>
        public Guid? RoleId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        //[SlugWithChinese(ErrorMessage = "名称只能包含中文、字母、数字、_和-，并且以中文或字母开头")]
        [StringLength(50, ErrorMessage = "名称请保持在50个字符以内")]
        [DisplayName("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 拥有权限 Id
        /// </summary>
        public Guid[] PermissionIds { get; set; }
    }

    /// <summary>
    /// 角色名称 Input
    /// </summary>
    public class RoleNameInput
    {
        /// <summary>
        /// 角色 Id
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        //[SlugWithChinese(ErrorMessage = "名称只能包含中文、字母、数字、_和-，并且以中文或字母开头")]
        [StringLength(50, ErrorMessage = "名称请保持在50个字符以内")]
        [DisplayName("名称")]
        public string Name { get; set; }
    }

    /// <summary>
    /// 移动角色 Input
    /// </summary>
    public class MoveRoleInput
    {
        /// <summary>
        /// 源 DisplayOrder
        /// </summary>
        public int SourceDisplayOrder { get; set; }

        /// <summary>
        /// 目标 DisplayOrder
        /// </summary>
        public int TargetDisplayOrder { get; set; }
    }
}
