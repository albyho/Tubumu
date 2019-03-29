using System;
using System.Collections.Generic;

namespace Tubumu.Modules.Admin.Domain.Entities
{
    public partial class Permission
    {
        public Permission()
        {
            GroupPermission = new HashSet<GroupPermission>();
            RolePermission = new HashSet<RolePermission>();
            UserPermission = new HashSet<UserPermission>();
        }

        public Guid? ParentId { get; set; }
        public Guid PermissionId { get; set; }
        public string ModuleName { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int DisplayOrder { get; set; }

        public virtual ICollection<GroupPermission> GroupPermission { get; set; }
        public virtual ICollection<RolePermission> RolePermission { get; set; }
        public virtual ICollection<UserPermission> UserPermission { get; set; }
    }
}
