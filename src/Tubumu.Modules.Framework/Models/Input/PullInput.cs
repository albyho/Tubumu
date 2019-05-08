using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Framework.Models
{
    /// <summary>
    /// 拉取 Input
    /// </summary>
    public class PullInput
    {
        /// <summary>
        /// 客户端最大 Id
        /// </summary>
        [DisplayName("客户端最大 Id")]
        [Range(0, Int32.MaxValue, ErrorMessage = "请输入合法的客户端最大 Id")]
        public int? MaxId { get; set; }

        /// <summary>
        /// 客户端最小Id
        /// </summary>
        [Range(0, Int32.MaxValue, ErrorMessage = "请输入合法的客户端最小 Id")]
        //[Mutex("MaxID",false/*, ErrorMessage = "客户端最大 Id 和客户端最小 Id 只能二选一"*/)]
        [DisplayName("客户端最小 Id")]
        public int? MinId { get; set; }

        /// <summary>
        /// 拉取数量
        /// </summary>
        [DisplayName("拉取数量")]
        [Range(1, 1000, ErrorMessage = "拉取数量请保持在 1-1000 之间")]
        public int PullCount { get; set; }
    }
}
