using System;
using System.Collections.Generic;
using XM = Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.Entities
{
    public partial class User
    {
        public User()
        {
            NotificationFromUser = new HashSet<Notification>();
            NotificationToUser = new HashSet<Notification>();
            NotificationUser = new HashSet<NotificationUser>();
            UserGroup = new HashSet<UserGroup>();
            UserPermission = new HashSet<UserPermission>();
            UserRole = new HashSet<UserRole>();
        }

        public Guid GroupId { get; set; }
        public Guid? RoleId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string RealName { get; set; }
        public bool RealNameIsValid { get; set; }
        public string Email { get; set; }
        public bool EmailIsValid { get; set; }
        public string Mobile { get; set; }
        public bool MobileIsValid { get; set; }
        public string Password { get; set; }
        public XM.UserStatus Status { get; set; }
        public DateTime CreationTime { get; set; }
        public string HeadUrl { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public string WeixinUnionId {get;set;}
        public string WeixinWebOpenId { get; set; }
        public string WeixinMobileEndOpenId { get; set; }
        public string WeixinAppOpenId { get; set; }
        public bool IsDeveloper { get; set; }
        public bool IsTester { get; set; }
        public byte[] RowVersion { get; set; }

        public virtual Group Group { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Notification> NotificationFromUser { get; set; }
        public virtual ICollection<Notification> NotificationToUser { get; set; }
        public virtual ICollection<NotificationUser> NotificationUser { get; set; }
        public virtual ICollection<UserGroup> UserGroup { get; set; }
        public virtual ICollection<UserPermission> UserPermission { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
