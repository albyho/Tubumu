using System;

namespace Tubumu.Modules.Framework.Swagger
{
    /// <summary>
    /// HiddenApiAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class HiddenApiAttribute : Attribute
    {
    }
}
