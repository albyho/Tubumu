using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Framework.ModelValidation.Attributes
{
    /// <summary>
    /// HttpUrlAttribute
    /// </summary>
    public class HttpUrlAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        // ^https?://(?:[^./\\s'\"<)\\]]+\\.)+[^./\\s'\"<\")\\]]+(?:/[^'\"<]*)*$
        // ^(http|ftp|https):\\/\\/[\\w\\-_]+(\\.[\\w\\-_]+)+([\\w\\-\\.,@?^=%&amp;:/~\\+#]*[\\w\\-\\@?^=%&amp;/~\\+#])?$
        // ^|[^\\w'\"]|\\G)(?<uri>(?:https?|ftp)(?:&#58;|:)(?:&#47;&#47;|//)(?:[^./\\s'\"<)\\]]+\\.)+[^./\\s'\"<)\\]]+(?:(?:&#47;|/).*?)?)(?:[\\s\\.,\\)\\]'\"]?(?:\\s|\\.|\\)|\\]|,|<|$)
        public HttpUrlAttribute() : base("^https?:\\/\\/[\\w\\-_]+(\\.[\\w\\-_]+)+([\\w\\-\\.,@?^=%&amp;:/~\\+#]*[\\w\\-\\@?^=%&amp;/~\\+#])?$")
        {
        }
    }
}
