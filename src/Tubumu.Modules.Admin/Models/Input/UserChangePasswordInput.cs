using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 用户修改密码 Input
    /// </summary>
    public class UserChangePasswordInput
    {
        /// <summary>
        /// 当前密码
        /// </summary>
        [Required(ErrorMessage = "当前密码不能为空")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "当前密码请保持在 6-32 个字符之间")]
        [DataType(DataType.Password)]
        [DisplayName("当前密码")]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// 新的密码
        /// </summary>
        [Required(ErrorMessage = "新的密码不能为空")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "新的密码请保持在 6-32 个字符之间")]
        [DataType(DataType.Password)]
        [DisplayName("新的密码")]
        public string NewPassword { get; set; }
        
        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "确认密码不能为空")]
        [StringLength(32, MinimumLength = 6, ErrorMessage = "确认密码请保持在 6-32 s个字符之间")]
        [Compare("NewPassword", ErrorMessage = "请确认两次输入的密码一致")]
        //[CompareAttribute("NewPassword", ValidationCompareOperator.Equal, ValidationDataType.String, ErrorMessage = "请确认两次输入的密码一致")]
        [DataType(DataType.Password)]
        [DisplayName("确认密码")]
        public string NewPasswordConfirm { get; set; }
    }
}
