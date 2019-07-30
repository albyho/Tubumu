using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Framework.ModelValidation.Attributes
{
    /// <summary>
    /// 字母或中文开头，由字母、数字、连词符或下滑线组成的字符串
    /// </summary>
    public class NameWithChineseAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// NameWithChineseAttribute
        /// </summary>
        public NameWithChineseAttribute() :
            base(@"^[a-zA-Z0-9-_\u4E00-\u9FA5\uF900-\uFA2D]*$")
        {
        }
    }
}
