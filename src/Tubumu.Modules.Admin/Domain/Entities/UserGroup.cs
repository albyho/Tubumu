using System;
using System.Collections.Generic;

namespace Tubumu.Modules.Admin.Domain.Entities
{
    public partial class UserGroup
    {
        public int UserId { get; set; }
        public Guid GroupId { get; set; }

        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}
