using System;
using System.Collections.Generic;

namespace Tubumu.Modules.Admin.Repositories.Entities
{
    public partial class NotificationUser
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public DateTime? ReadTime { get; set; }
        public DateTime? DeleteTime { get; set; }

        public virtual Notification Notification { get; set; }
        public virtual User User { get; set; }
    }
}
