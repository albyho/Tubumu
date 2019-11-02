using System;

namespace Tubumu.Swagger
{
    /// <summary>
    /// HiddenApiAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class HiddenApiAttribute : Attribute
    {
    }

    /// <summary>
    /// IgnoreHiddenApiAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class IgnoreHiddenApiAttribute : Attribute
    {
    }
}
