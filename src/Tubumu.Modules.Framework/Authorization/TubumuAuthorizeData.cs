using System;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Tubumu.Modules.Framework.Authorization
{
    /// <summary>
    /// TubumuAuthorizeData
    /// </summary>
    public class TubumuAuthorizeData : ITubumuAuthorizeData
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
