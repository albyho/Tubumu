using System;

namespace Tubumu.Modules.Framework.Authorization
{
    public class Role
    {
        public Guid RoleId { get; set; }

        public string Name { get; set; }

        public Guid[] PermissionIds { get; set; }
    }
}
