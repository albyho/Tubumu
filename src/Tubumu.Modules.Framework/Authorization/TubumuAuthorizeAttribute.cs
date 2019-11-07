using System;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Tubumu.Modules.Framework.Authorization
{
    /// <summary>
    /// TubumuAuthorizeAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class TubumuAuthorizeAttribute : Attribute, ITubumuAuthorizeData
    {
        const string PolicyPrefix = "Tubumu:";
        private TubumuAuthorizeData _authorizeData = new TubumuAuthorizeData();

        /// <summary>
        /// Policy
        /// </summary>
        public string Policy { get; set; }

        /// <summary>
        /// AuthenticationSchemes
        /// </summary>
        public string AuthenticationSchemes { get; set; }

        /// <summary>
        /// Groups
        /// </summary>
        public string Groups
        {
            get
            {
                return _authorizeData.Groups;
            }
            set
            {
                _authorizeData.Groups = value;
                Policy = $"{PolicyPrefix}{JsonConvert.SerializeObject(_authorizeData)}";
            }
        }

        /// <summary>
        /// Roles
        /// </summary>
        public string Roles
        {
            get
            {
                return _authorizeData.Roles;
            }
            set
            {
                _authorizeData.Roles = value;
                Policy = $"{PolicyPrefix}{JsonConvert.SerializeObject(_authorizeData)}";
            }
        }

        /// <summary>
        /// Permissions
        /// </summary>
        public string Permissions
        {
            get
            {
                return _authorizeData.Permissions;
            }
            set
            {
                _authorizeData.Permissions = value;
                Policy = $"{PolicyPrefix}{JsonConvert.SerializeObject(_authorizeData)}";
            }
        }
    }
}
