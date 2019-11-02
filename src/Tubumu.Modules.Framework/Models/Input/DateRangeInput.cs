using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Tubumu.DataAnnotations;

namespace Tubumu.Modules.Framework.Models
{
    /// <summary>
    /// DateRange input
    /// </summary>
    public class DateRangeInput
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        [Required(ErrorMessage = "请选择开始日期")]
        [DisplayName("开始日期")]
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        [Required(ErrorMessage = "请选择结束日期")]
        [Tubumu.DataAnnotations.Compare("BeginDate", ValidationCompareOperator.GreaterThanEqual, ValidationDataType.Date, ErrorMessage = "结束日期不能小于开始日期")]
        [DisplayName("结束日期")]
        public DateTime EndDate { get; set; }
    }
}
