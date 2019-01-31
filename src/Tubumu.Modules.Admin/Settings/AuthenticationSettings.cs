using System;
using System.Collections.Generic;
using System.Text;
using Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.Settings
{
    public class AuthenticationSettings
    {
        public Guid RegisterDefaultGroupId { get; set; }
        public UserStatus RegisterDefaultStatus { get; set; }

        public AuthenticationSettings()
        {
            RegisterDefaultGroupId = new Guid("11111111-1111-1111-1111-111111111111"); // 等待分组组
            RegisterDefaultStatus = UserStatus.Normal;
        }
    }
}
