using System;
using System.Collections.Generic;

namespace Tubumu.Modules.Admin.Repositories.Entities
{
    public partial class GroupPermission
    {
        public Guid GroupId { get; set; }
        public Guid PermissionId { get; set; }

        public virtual Group Group { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
