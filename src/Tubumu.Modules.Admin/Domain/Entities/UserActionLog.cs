using System;
using System.Collections.Generic;

namespace Tubumu.Modules.Admin.Domain.Entities
{
    public partial class UserActionLog
    {
        public int UserActionLogId { get; set; }
        public int UserId { get; set; }
        public int ActionTypeId { get; set; }
        public int? ClientTypeId { get; set; }
        public string ClientAgent { get; set; }
        public string Remark { get; set; }
        public DateTime CreationTime { get; set; }

        public virtual User User { get; set; }
    }
}
