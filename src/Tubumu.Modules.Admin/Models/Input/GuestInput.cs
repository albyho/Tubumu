using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Admin.Models.Input
{
    /// <summary>
    /// GuestInput
    /// </summary>
    public class GuestInput : ClientTypeInput
    {
        /// <summary>
        /// 唯一识别码
        /// </summary>
        [Required(ErrorMessage = "请输入 UniqueId")]
        [StringLength(50, ErrorMessage = "UniqueId 请保持在50个字符以内")]
        public string UniqueId { get; set; }
    }
}
