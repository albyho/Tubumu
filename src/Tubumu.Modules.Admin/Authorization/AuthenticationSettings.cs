using System;
using Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.Authorization
{
    /// <summary>
    /// AuthenticationSettings
    /// </summary>
    public class AuthenticationSettings
    {
        /// <summary>
        /// RegisterDefaultGroupId
        /// </summary>
        public Guid RegisterDefaultGroupId { get; set; }

        /// <summary>
        /// RegisterDefaultStatus
        /// </summary>
        public UserStatus RegisterDefaultStatus { get; set; }

        /// <summary>
        /// Consturctor
        /// </summary>
        public AuthenticationSettings()
        {
            RegisterDefaultGroupId = new Guid("11111111-1111-1111-1111-111111111111"); // 等待分配组
            RegisterDefaultStatus = UserStatus.Normal;
        }
    }
}
