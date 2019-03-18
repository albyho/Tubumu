using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Tubumu.Modules.Framework.ModelValidation.Attributes
{
    /// <summary>
    /// 虚拟根路径,如:~/, ~/WebSite, ~/WebSite/abc/123
    /// </summary>
    public class VirtualDirectoryAttribute : RegularExpressionAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// ^~(/[a-zA-Z0-9-_]+)+$ 匹配 ~/abc，但是不匹配 ~/
        /// ^~(/|(/[a-zA-Z0-9-_]+)*)$
        public VirtualDirectoryAttribute() :
            base(@"^~(/[a-zA-Z0-9-_]+)+$")
        {
        }
    }
}
