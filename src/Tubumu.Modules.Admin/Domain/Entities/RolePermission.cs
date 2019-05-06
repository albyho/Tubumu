using System;

namespace Tubumu.Modules.Admin.Domain.Entities
{
    public partial class RolePermission
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual Role Role { get; set; }
    }
}
