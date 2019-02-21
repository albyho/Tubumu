using System;

namespace Tubumu.Modules.Framework.Authorization
{
    /// <summary>
    /// PermissionAuthorizeAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionAuthorizeAttribute : Attribute, IPermissionAuthorizeData
    {
        /// <summary>
        /// Policy
        /// </summary>
        public string Policy { get; set; }

        /// <summary>
        /// Roles
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// AuthenticationSchemes
        /// </summary>
        public string AuthenticationSchemes { get; set; }

        /// <summary>
        /// Rule
        /// </summary>
        public string Rule { get; set; }

        /// <summary>
        /// Groups
        /// </summary>
        public string Groups { get; set; }

        /// <summary>
        /// Permissions
        /// </summary>
        public string Permissions { get; set; }
    }
}
