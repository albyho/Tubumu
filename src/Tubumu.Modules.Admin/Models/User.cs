using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Tubumu.Core.Extensions;

namespace Tubumu.Modules.Admin.Models
{
    /// <summary>
    /// 用户信息包装(简单信息)
    /// </summary>
    public class UserInfoWarpper
    {
        /// <summary>
        /// 用户 Id
        /// </summary>
        [JsonProperty(PropertyName = "userId")]
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// AvatarUrl
        /// </summary>
        [JsonProperty(PropertyName = "avatarUrl")]
        public string AvatarUrl { get; set; }

        /// <summary>
        /// LogoUrl
        /// </summary>
        [JsonProperty(PropertyName = "logoUrl")]
        public string LogoUrl { get; set; }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class Profile : UserInfoWarpper
    {
        /// <summary>
        /// 附加分组
        /// </summary>
        [JsonProperty(PropertyName = "groups")]
        public IEnumerable<GroupInfo> Groups { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [JsonConverter(typeof(Tubumu.Core.Json.NullValueJsonConverterGuid), "RoleId", "00000000-0000-0000-0000-000000000000")]
        [JsonProperty(PropertyName = "role")]
        public RoleInfo Role { get; set; }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoProfile
    {
        /// <summary>
        /// 用户 Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// AvatarUrl
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// LogoUrl
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// 真实名称
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 真实名称是否已经验证
        /// </summary>
        public bool RealNameIsValid { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 邮箱是否已经验证
        /// </summary>
        public bool EmailIsValid { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 手机号是否已经验证
        /// </summary>
        public bool MobileIsValid { get; set; }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserInfoBase()
        {
            Groups = new HashSet<GroupInfo>();
        }

        /// <summary>
        /// 用户 Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// LogoUrl
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// 真实名称
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 真实名称是否已经验证
        /// </summary>
        public bool RealNameIsValid { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 邮箱是否已经验证
        /// </summary>
        public bool EmailIsValid { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 手机号码是否已经验证
        /// </summary>
        public bool MobileIsValid { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public UserStatus Status { get; set; }

        /// <summary>
        /// 文本形式的用户状态
        /// </summary>
        public string StatusText => Status.GetEnumDisplayName();

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// AvatarUrl
        /// </summary>
        public string AvatarUrl { get; set; }

        /// <summary>
        /// WeixinMobileEndOpenId
        /// </summary>
        public string WeixinMobileEndOpenId { get; set; }

        /// <summary>
        /// WeixinAppOpenId
        /// </summary>
        public string WeixinAppOpenId { get; set; }

        /// <summary>
        /// WeixinWebOpenId
        /// </summary>
        public string WeixinWebOpenId { get; set; }

        /// <summary>
        /// WeixinUnionId
        /// </summary>
        public string WeixinUnionId { get; set; }

        /// <summary>
        /// 是否是开发人员
        /// </summary>
        public bool IsDeveloper { get; set; }

        /// <summary>
        /// 是否是测试人员
        /// </summary>
        public bool IsTester { get; set; }

        /// <summary>
        /// 主要分组
        /// </summary>
        public GroupInfo Group { get; set; }

        /// <summary>
        /// 所属附件分组
        /// </summary>
        public IEnumerable<GroupInfo> Groups { get; set; }

        /// <summary>
        /// 主要角色
        /// </summary>
        [JsonConverter(typeof(Tubumu.Core.Json.NullValueJsonConverterGuid), "RoleId", "00000000-0000-0000-0000-000000000000")]
        public RoleInfo Role { get; set; }

        /// <summary>
        /// 用户拥有的特定权限
        /// </summary>
        public IEnumerable<PermissionBase> Permissions { get; set; }

        /// <summary>
        /// DisplayName 并包含 RealName 的名称
        /// </summary>
        public string FullDisplayName
        {
            get
            {
                if (!DisplayName.IsNullOrWhiteSpace() && !RealName.IsNullOrWhiteSpace())
                {
                    return $"{DisplayName}({RealName})";
                }
                else if (!DisplayName.IsNullOrWhiteSpace())
                {
                    return DisplayName;
                }
                else if (!RealName.IsNullOrWhiteSpace())
                {
                    return DisplayName;
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        /// <summary>
        /// DisplayName 优先，其次 RealName 的名称
        /// </summary>
        public string DisplayNameRealName
        {
            get
            {
                if (!DisplayName.IsNullOrWhiteSpace())
                {
                    return DisplayName;
                }
                else if (!RealName.IsNullOrWhiteSpace())
                {
                    return RealName;
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        /// <summary>
        /// RealName 优先，其次 DisplayName 的名称
        /// </summary>
        public string RealNameDisplayNme
        {
            get
            {
                if (!RealName.IsNullOrWhiteSpace())
                {
                    return RealName;
                }
                else if (!DisplayName.IsNullOrWhiteSpace())
                {
                    return DisplayName;
                }
                else
                {
                    return String.Empty;
                }
            }
        }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo : UserInfoBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UserInfo()
        {
            Roles = new HashSet<RoleInfo>();
            GroupRoles = new HashSet<RoleInfo>();
            GroupsRoles = new HashSet<RoleInfo>();
            GroupPermissions = new HashSet<PermissionBase>();
            GroupsPermissions = new HashSet<PermissionBase>();
            GroupRolesPermissions = new HashSet<PermissionBase>();
            GroupsRolesPermissions = new HashSet<PermissionBase>();
            RolePermissions = new HashSet<PermissionBase>();
            RolesPermissions = new HashSet<PermissionBase>();
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 用户拥有的附加角色
        /// </summary>
        public IEnumerable<RoleInfo> Roles { get; set; }

        /// <summary>
        /// 用户主要分组所拥有的角色
        /// </summary>
        public IEnumerable<RoleInfo> GroupRoles { get; set; }

        /// <summary>
        /// 用户附加分组所拥有的角色
        /// </summary>
        public IEnumerable<RoleInfo> GroupsRoles { get; set; }

        /// <summary>
        /// 用户主要分组所拥有的权限
        /// </summary>
        public IEnumerable<PermissionBase> GroupPermissions { get; set; }

        /// <summary>
        /// 用户附加分组所拥有的权限
        /// </summary>
        public IEnumerable<PermissionBase> GroupsPermissions { get; set; }

        /// <summary>
        /// 用户主要分组所拥有的角色所拥有的权限
        /// </summary>
        public IEnumerable<PermissionBase> GroupRolesPermissions { get; set; }

        /// <summary>
        /// 用户附加分组所拥有的角色所拥有的权限
        /// </summary>
        public IEnumerable<PermissionBase> GroupsRolesPermissions { get; set; }

        /// <summary>
        /// 用户的主要角色所拥有的权限
        /// </summary>
        public IEnumerable<PermissionBase> RolePermissions { get; set; }

        /// <summary>
        /// 用户的附加角色所拥有的权限
        /// </summary>
        public IEnumerable<PermissionBase> RolesPermissions { get; set; }

        /// <summary>
        /// 所有分组
        /// </summary>
        [JsonIgnore]
        public IEnumerable<GroupInfo> AllGroups
        {
            get
            {
                yield return Group;
                foreach (var item in Groups)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// 所有角色
        /// </summary>
        [JsonIgnore]
        public IEnumerable<RoleInfo> AllRoles
        {
            get
            {
                if (Role != null && Role.RoleId != Guid.Empty)
                {
                    yield return Role;
                }
                foreach (var item in Roles)
                {
                    yield return item;
                }

                foreach (var item in GroupRoles)
                {
                    yield return item;
                }

                foreach (var item in GroupsRoles)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// 所有权限
        /// </summary>
        [JsonIgnore]
        public IEnumerable<PermissionBase> AllPermissions
        {
            get
            {
                foreach (var item in Permissions)
                {
                    yield return item;
                }
                foreach (var item in GroupPermissions)
                {
                    yield return item;
                }
                foreach (var item in GroupsPermissions)
                {
                    yield return item;
                }
                foreach (var item in GroupRolesPermissions)
                {
                    yield return item;
                }
                foreach (var item in GroupsRolesPermissions)
                {
                    yield return item;
                }
                foreach (var item in RolePermissions)
                {
                    yield return item;
                }
                foreach (var item in RolesPermissions)
                {
                    yield return item;
                }
            }
        }
    }
}