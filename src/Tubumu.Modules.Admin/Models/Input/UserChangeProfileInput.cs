using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Tubumu.Modules.Framework.ModelValidation.Attributes;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 用户修改资料 Input
    /// </summary>
    public class UserChangeProfileInput
    {
        /// <summary>
        /// 显示名称（昵称）
        /// </summary>
        //public string Username { get; set; }
        //[Required(ErrorMessage = "昵称不能为空")]
        [StringLength(20, ErrorMessage = "昵称请保持在20个字符以内")]
        [SlugWithChinese(ErrorMessage = "以字母或中文开头的中文字母数字_和-组成")]
        [DisplayName("昵称")]
        public string DisplayName { get; set; }
    }
}
