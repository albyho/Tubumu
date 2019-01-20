using System;

namespace Tubumu.Modules.Framework.Authorization
{
    public class Group
    {
        public Guid GroupId { get; set; }

        public Guid? ParentId { get; set; }

        public string Name { get; set; }

        public bool IsContainsUser { get; set; }

        public bool IsDisabled { get; set; }

        public Guid[] RoleIds { get; set; }

        public Guid[] AvailableRoleIds { get; set; }

        public Guid[] PermissionIds { get; set; }
    }
}
