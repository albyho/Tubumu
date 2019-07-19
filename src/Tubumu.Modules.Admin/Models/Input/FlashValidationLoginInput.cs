using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// 闪验登录
    /// </summary>
    public class FlashValidationLoginInput : ClientTypeInput
    {
        //[Required(ErrorMessage = "请输入 AppId")]
        //public string AppId { get; set; }

        [Required(ErrorMessage = "请输入 AccessToken")]
        public string AccessToken { get; set; }

        [Required(ErrorMessage = "请输入 Telecom")]
        public string Telecom { get; set; }

        [Required(ErrorMessage = "请输入 Timestamp")]
        public string Timestamp { get; set; }

        [Required(ErrorMessage = "请输入 Randoms")]
        public string Randoms { get; set; }

        [Required(ErrorMessage = "请输入 Version")]
        public string Version { get; set; }

        [Required(ErrorMessage = "请输入 Sign")]
        public string Sign { get; set; }

        [Required(ErrorMessage = "请输入 Device")]
        public string Device { get; set; }
    }
}
