using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Framework.ModelValidation.Attributes
{
    /// <summary>
    /// 子路径，如：abc,abc/edf匹配;而/,/abc,abc/,abc/edf/不匹配
    /// </summary>
    /// <example>
    /// <para>abc</para>
    /// <para>abc/edf</para>
    /// </example>
    public class SubDirectoryAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SubDirectoryAttribute() :
            base(@"^[a-zA-Z0-9-_]+(/[a-zA-Z0-9-_]+)*$")
        { }
    }
}
