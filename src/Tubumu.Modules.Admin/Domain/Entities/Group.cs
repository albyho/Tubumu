using System;
using System.Collections.Generic;

namespace Tubumu.Modules.Admin.Domain.Entities
{
    public partial class Group
    {
        public Group()
        {
            GroupAvailableRole = new HashSet<GroupAvailableRole>();
            GroupPermission = new HashSet<GroupPermission>();
            GroupRole = new HashSet<GroupRole>();
            User = new HashSet<User>();
            UserGroup = new HashSet<UserGroup>();
        }

        public Guid? ParentId { get; set; }
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsContainsUser { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsSystem { get; set; }

        public virtual ICollection<GroupAvailableRole> GroupAvailableRole { get; set; }
        public virtual ICollection<GroupPermission> GroupPermission { get; set; }
        public virtual ICollection<GroupRole> GroupRole { get; set; }
        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<UserGroup> UserGroup { get; set; }
    }
}
