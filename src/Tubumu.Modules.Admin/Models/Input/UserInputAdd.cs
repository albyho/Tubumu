using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 用户添加 Input
    /// </summary>
    public class UserInputAdd : UserInput
    {
        /// <summary>
        /// 登录密码
        /// </summary>
        [Required(ErrorMessage = "登录密码不能为空")]
        public override string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "确认密码不能为空")]
        public override string PasswordConfirm { get; set; }
    }
}
