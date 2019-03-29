using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tubumu.Modules.Admin.Domain.Entities
{
    public partial class TubumuContext : DbContext
    {
        public TubumuContext()
        {
        }

        public TubumuContext(DbContextOptions<TubumuContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bulletin> Bulletin { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<GroupAvailableRole> GroupAvailableRole { get; set; }
        public virtual DbSet<GroupPermission> GroupPermission { get; set; }
        public virtual DbSet<GroupRole> GroupRole { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<NotificationUser> NotificationUser { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RolePermission> RolePermission { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserGroup> UserGroup { get; set; }
        public virtual DbSet<UserPermission> UserPermission { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Tubumu;User Id=sa;Password=123456;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Bulletin>(entity =>
            {
                entity.Property(e => e.BulletinId).ValueGeneratedNever();

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.Title).HasMaxLength(200);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.GroupId).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<GroupAvailableRole>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.RoleId });

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupAvailableRole)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_GroupAvailableRole_Group_AvailableRoles");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.GroupAvailableRole)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_GroupAvailableRole_Role_AvailableGroups");
            });

            modelBuilder.Entity<GroupPermission>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.PermissionId });

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupPermission)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_GroupPermission_Group");

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.GroupPermission)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_GroupPermission_Permission");
            });

            modelBuilder.Entity<GroupRole>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.RoleId });

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupRole)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_GroupRole_Group");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.GroupRole)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_GroupRole_Role");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Url).HasMaxLength(200);

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.NotificationFromUser)
                    .HasForeignKey(d => d.FromUserId)
                    .HasConstraintName("FK_Notification_User_FromUserNotifications");

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.NotificationToUser)
                    .HasForeignKey(d => d.ToUserId)
                    .HasConstraintName("FK_Notification_User_ToUserNotifications");
            });

            modelBuilder.Entity<NotificationUser>(entity =>
            {
                entity.HasKey(e => new { e.NotificationId, e.UserId })
                    .HasName("PK_NotificationUserReaded");

                entity.HasOne(d => d.Notification)
                    .WithMany(p => p.NotificationUser)
                    .HasForeignKey(d => d.NotificationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NotificationUser_Notification");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.NotificationUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NotificationUser_User");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.PermissionId).ValueGeneratedNever();

                entity.Property(e => e.ModuleName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.Property(e => e.RegionId).ValueGeneratedNever();

                entity.Property(e => e.Extra)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Initial)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Initials)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Pinyin)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RegionCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.Suffix)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Region_Region");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.PermissionId });

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.RolePermission)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_RolePermission_Permission");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RolePermission)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_RolePermission_Role");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username)
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.DisplayName).HasMaxLength(20);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.HeadUrl).HasMaxLength(200);

                entity.Property(e => e.LogoUrl).HasMaxLength(200);

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RealName).HasMaxLength(20);

                entity.Property(e => e.RowVersion)
                    .IsRequired()
                    .IsRowVersion();

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.WeixinAppOpenId).HasMaxLength(50);

                entity.Property(e => e.WeixinMobileEndOpenId).HasMaxLength(50);

                entity.Property(e => e.WeixinWebOpenId).HasMaxLength(50);

                entity.Property(e => e.WeixinUnionId).HasMaxLength(50);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_User_Group");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.GroupId });

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.UserGroup)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserGroup_Group");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserGroup)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserGroup_User");
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.PermissionId });

                entity.HasOne(d => d.Permission)
                    .WithMany(p => p.UserPermission)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_UserPermission_Permission");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPermission)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserPermission_User");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UserRole_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_User");
            });
        }
    }
}
