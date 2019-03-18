using System;
using System.Collections.Generic;

namespace Tubumu.Modules.Admin.Repositories.Entities
{
    public partial class Role
    {
        public Role()
        {
            GroupAvailableRole = new HashSet<GroupAvailableRole>();
            GroupRole = new HashSet<GroupRole>();
            RolePermission = new HashSet<RolePermission>();
            User = new HashSet<User>();
            UserRole = new HashSet<UserRole>();
        }

        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsSystem { get; set; }

        public virtual ICollection<GroupAvailableRole> GroupAvailableRole { get; set; }
        public virtual ICollection<GroupRole> GroupRole { get; set; }
        public virtual ICollection<RolePermission> RolePermission { get; set; }
        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
