using System;
using System.Collections.Generic;

namespace Tubumu.Modules.Admin.Domain.Entities
{
    public partial class GroupAvailableRole
    {
        public Guid GroupId { get; set; }
        public Guid RoleId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Role Role { get; set; }
    }
}
