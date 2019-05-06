using System;

namespace Tubumu.Modules.Admin.Domain.Entities
{
    public partial class UserRole
    {
        public int UserId { get; set; }
        public Guid RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
