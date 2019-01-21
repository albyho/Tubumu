using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Framework.ModelValidation.Attributes
{
    public class HourMinuteAttribute : RegularExpressionAttribute
    {
        // 匹配：
        // 单个时段：08:30-09:10
        // 多个时段：08:30-09:10 18:00-18:40
        // 任意空格：08:30-09:10    18:00-18:40   23:10-23:30
        public HourMinuteAttribute() :
            base(@"^([01]\d|2[0-3]):([0-5][0-9])$") { }
    }
}
