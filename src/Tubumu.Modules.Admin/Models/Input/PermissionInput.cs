using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Tubumu.Modules.Framework.ModelValidation.Attributes;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 权限 Id Input
    /// </summary>
    public class PermissionIdInput
    {
        /// <summary>
        /// 权限 Id
        /// </summary>
        [Required(ErrorMessage = "请输入权限 Id")]
        public int PermissionId { get; set; }
    }

    /// <summary>
    /// 权限 Input
    /// </summary>
    public class PermissionInput
    {
        /// <summary>
        /// 权限 Id
        /// </summary>
        [DisplayName("权限 Id")]
        public Guid? PermissionId { get; set; }

        /// <summary>
        /// 所属权限（父权限）
        /// </summary>
        [DisplayName("所属权限")]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        [Required(ErrorMessage = "模块名称不能为空")]
        [StringLength(50, ErrorMessage = "模块名称请保持在50个字符以内")]
        [SlugWithChinese(ErrorMessage = "模块名称只能包含中文、字母、数字、_和-，并且以中文或字母开头")]
        [DisplayName("模块名称")]
        public string ModuleName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(50, ErrorMessage = "名称请保持在50个字符以内")]
        [SlugWithChinese(ErrorMessage = "名称只能包含中文、字母、数字、_和-，并且以中文或字母开头")]
        [DisplayName("名称")]
        public string Name { get; set; }
    }
}
