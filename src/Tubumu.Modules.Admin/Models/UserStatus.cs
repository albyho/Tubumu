using System.ComponentModel.DataAnnotations;
namespace Tubumu.Modules.Admin.Models
{
    /// <summary>
    /// 用户状态
    /// <remark>Normal: 默认 Normal: 待审 PendingApproval: 待删</remark>
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Display(Name = "默认")]
        Normal = 1,

        /// <summary>
        /// 待审
        /// </summary>
        [Display(Name = "待审")]
        PendingApproval = 2,

        /// <summary>
        /// 待删
        /// </summary>
        [Display(Name = "待删")]
        Removed = 3
    }
}
