using System;
using System.Collections.Generic;

namespace Tubumu.Modules.Admin.Repositories.Entities
{
    public partial class Notification
    {
        public Notification()
        {
            NotificationUser = new HashSet<NotificationUser>();
        }

        public int NotificationId { get; set; }
        public int? FromUserId { get; set; }
        public int? ToUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }

        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
        public virtual ICollection<NotificationUser> NotificationUser { get; set; }
    }
}
