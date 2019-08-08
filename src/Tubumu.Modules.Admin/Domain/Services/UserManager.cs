using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Admin.Domain.Entities;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Core.Models;
using Tubumu.Modules.Framework.Extensions;
using XM = Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.Domain.Services
{
    /// <summary>
    /// IUserManager
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// GetItemByUserIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<XM.UserInfo> GetItemByUserIdAsync(int userId, XM.UserStatus? status = null);

        /// <summary>
        /// GetItemByUsernameAsync
        /// </summary>
        /// <param name="username"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<XM.UserInfo> GetItemByUsernameAsync(string username, XM.UserStatus? status = null);

        /// <summary>
        /// GetItemByEmailAsync
        /// </summary>
        /// <param name="email"></param>
        /// <param name="emailIsValid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<XM.UserInfo> GetItemByEmailAsync(string email, bool? emailIsValid, XM.UserStatus? status = null);

        /// <summary>
        /// GetItemByMobileAsync
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="mobileIsValid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<XM.UserInfo> GetItemByMobileAsync(string mobile, bool? mobileIsValid, XM.UserStatus? status = null);

        /// <summary>
        /// GetUserInfoWarpperListAsync
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        Task<List<XM.UserInfoWarpper>> GetUserInfoWarpperListAsync(IEnumerable<int> userIds);

        /// <summary>
        /// GetPageAsync
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<Page<XM.UserInfo>> GetPageAsync(XM.UserPageSearchCriteria criteria);

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<XM.UserInfo> SaveAsync(UserInput userInput, ModelStateDictionary modelState);

        /// <summary>
        /// ChangeUsernameAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newUsername"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangeUsernameAsync(int userId, string newUsername, ModelStateDictionary modelState);

        /// <summary>
        /// ChangeDisplayNameAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="displayName"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangeDisplayNameAsync(int userId, string displayName, ModelStateDictionary modelState);

        /// <summary>
        /// ChangePasswordAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangePasswordAsync(int userId, string password, ModelStateDictionary modelState);

        /// <summary>
        /// ChangeProfileAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userChangeProfileInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangeProfileAsync(int userId, UserChangeProfileInput userChangeProfileInput, ModelStateDictionary modelState);

        /// <summary>
        /// ChangePasswordAsync
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<int> ChangePasswordAsync(string username, string password, ModelStateDictionary modelState);

        /// <summary>
        /// ResetPasswordByAccountAsync
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<int> ResetPasswordByAccountAsync(string account, string password, ModelStateDictionary modelState);

        /// <summary>
        /// RemoveAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(int userId, ModelStateDictionary modelState);

        /// <summary>
        /// ChangeStatusAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangeStatusAsync(int userId, XM.UserStatus status, ModelStateDictionary modelState);

        #region UniqueId

        /// <summary>
        /// GetItemByUniqueIdAsync
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        Task<XM.UserInfo> GetItemByUniqueIdAsync(string uniqueId);

        /// <summary>
        /// GetOrGenerateItemByUniqueIdAsync
        /// </summary>
        /// <param name="generateGroupId"></param>
        /// <param name="generateStatus"></param>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        Task<XM.UserInfo> GetOrGenerateItemByUniqueIdAsync(Guid generateGroupId, XM.UserStatus generateStatus, string uniqueId);

        #endregion

        #region Verify exists

        /// <summary>
        /// IsExistsUsernameAsync
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<bool> IsExistsUsernameAsync(string username);

        /// <summary>
        /// IsExistsEmailAsync
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<bool> IsExistsEmailAsync(string email);

        /// <summary>
        /// IsExistsMobileAsync
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        Task<bool> IsExistsMobileAsync(string mobile);

        /// <summary>
        /// IsExistsAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(int userId, XM.UserStatus? status = null);

        /// <summary>
        /// VerifyExistsUsernameAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<bool> VerifyExistsUsernameAsync(int userId, string username);

        /// <summary>
        /// VerifyExistsMobileAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        Task<bool> VerifyExistsMobileAsync(int userId, string mobile);

        /// <summary>
        /// VerifyExistsEmailAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<bool> VerifyExistsEmailAsync(int userId, string email);

        /// <summary>
        /// VerifyExistsAsync
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> VerifyExistsAsync(UserInput userInput, ModelStateDictionary modelState);

        #endregion

        #region Avatar & Logo

        /// <summary>
        /// GetAvatarUrlAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetAvatarUrlAsync(int userId);

        /// <summary>
        /// GetLogoUrlAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetLogoUrlAsync(int userId);

        /// <summary>
        /// ChangeAvatarAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="avatarUrl"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangeAvatarAsync(int userId, string avatarUrl, ModelStateDictionary modelState);

        /// <summary>
        /// ChangeLogoAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="logoUrl"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> ChangeLogoAsync(int userId, string logoUrl, ModelStateDictionary modelState);

        #endregion
    }

    /// <summary>
    /// UserManager
    /// </summary>
    public class UserManager : IUserManager
    {
        private readonly TubumuContext _context;
        private readonly Expression<Func<User, XM.UserInfo>> _selector;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public UserManager(TubumuContext context)
        {
            _context = context;
            _selector = u => new XM.UserInfo
            {
                UserId = u.UserId,
                Username = u.Username,
                DisplayName = u.DisplayName,
                LogoUrl = u.LogoUrl,
                RealName = u.RealName,
                RealNameIsValid = u.RealNameIsValid,
                Email = u.Email,
                EmailIsValid = u.EmailIsValid,
                Mobile = u.Mobile,
                MobileIsValid = u.MobileIsValid,
                Password = u.Password,
                UniqueId = u.UniqueId,
                WeixinMobileEndOpenId = u.WeixinMobileEndOpenId,
                WeixinAppOpenId = u.WeixinAppOpenId,
                WeixinWebOpenId = u.WeixinWebOpenId,
                WeixinUnionId = u.WeixinUnionId,
                CreationTime = u.CreationTime,
                Description = u.Description,
                Status = u.Status,
                AvatarUrl = u.AvatarUrl,
                IsDeveloper = u.IsDeveloper,
                IsTester = u.IsTester,
                Group = new XM.GroupInfo
                {
                    GroupId = u.Group.GroupId,
                    Name = u.Group.Name,
                },
                Groups = from ur in u.UserGroup
                         select new XM.GroupInfo
                         {
                             GroupId = ur.GroupId,
                             Name = ur.Group.Name,
                         },
                Role = u.Role != null ? new XM.RoleInfo
                {
                    RoleId = u.Role.RoleId,
                    Name = u.Role.Name,
                } : null,
                Roles = from ur in u.UserRole
                        select new XM.RoleInfo
                        {
                            RoleId = ur.RoleId,
                            Name = ur.Role.Name,
                        },
                GroupRoles = from ugr in u.Group.GroupRole
                             select new XM.RoleInfo
                             {
                                 RoleId = ugr.RoleId,
                                 Name = ugr.Role.Name,
                             },
                GroupsRoles =
                    from ug in u.UserGroup
                    from ugr in ug.Group.GroupRole
                    select new XM.RoleBase
                    {
                        RoleId = ugr.RoleId,
                        Name = ugr.Role.Name,
                        IsSystem = ugr.Role.IsSystem,
                        DisplayOrder = ugr.Role.DisplayOrder
                    },
                Permissions = from up in u.UserPermission
                              select new XM.PermissionBase
                              {
                                  ModuleName = up.Permission.ModuleName,
                                  PermissionId = up.PermissionId,
                                  Name = up.Permission.Name
                              },
                GroupPermissions = from upp in u.Group.GroupPermission
                                   select new XM.PermissionBase
                                   {
                                       ModuleName = upp.Permission.ModuleName,
                                       PermissionId = upp.PermissionId,
                                       Name = upp.Permission.Name
                                   },
                GroupsPermissions = from gs in u.UserGroup
                                    from upp in gs.Group.GroupPermission
                                    select new XM.PermissionBase
                                    {
                                        ModuleName = upp.Permission.ModuleName,
                                        PermissionId = upp.PermissionId,
                                        Name = upp.Permission.Name
                                    },
                RolePermissions = from ur in u.Role.RolePermission
                                  where u.Role != null
                                  select new XM.PermissionBase
                                  {
                                      ModuleName = ur.Permission.ModuleName,
                                      PermissionId = ur.PermissionId,
                                      Name = ur.Permission.Name
                                  },
                RolesPermissions = from usr in u.UserRole
                                   from urp in usr.Role.RolePermission
                                   select new XM.PermissionBase
                                   {
                                       ModuleName = urp.Permission.ModuleName,
                                       PermissionId = urp.PermissionId,
                                       Name = urp.Permission.Name
                                   },
                GroupRolesPermissions = from gr in u.Group.GroupRole
                                        from p in gr.Role.RolePermission
                                        select new XM.PermissionBase
                                        {
                                            ModuleName = p.Permission.ModuleName,
                                            PermissionId = p.PermissionId,
                                            Name = p.Permission.Name
                                        },
                GroupsRolesPermissions =
                    from ug in u.UserGroup
                    from usr in ug.Group.GroupRole
                    from urp in usr.Role.RolePermission
                    select new XM.PermissionBase
                    {
                        ModuleName = urp.Permission.ModuleName,
                        PermissionId = urp.PermissionId,
                        Name = urp.Permission.Name
                    },
            };
        }

        /// <summary>
        /// GetItemByUserIdAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<XM.UserInfo> GetItemByUserIdAsync(int userId, XM.UserStatus? status = null)
        {
            XM.UserInfo user;
            if (status.HasValue)
            {
                user = await _context.User.AsNoTracking().Select(_selector).FirstOrDefaultAsync(m => m.UserId == userId && m.Status == status.Value);
            }
            else
            {
                user = await _context.User.AsNoTracking().Select(_selector).FirstOrDefaultAsync(m => m.UserId == userId);
            }
            return user;
        }

        /// <summary>
        /// GetItemByUsernameAsync
        /// </summary>
        /// <param name="username"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<XM.UserInfo> GetItemByUsernameAsync(string username, XM.UserStatus? status = null)
        {
            XM.UserInfo user;
            if (status.HasValue)
            {
                user = await _context.User.AsNoTracking().Select(_selector).FirstOrDefaultAsync(m => m.Username == username && m.Status == status.Value);
            }
            else
            {
                user = await _context.User.AsNoTracking().Select(_selector).FirstOrDefaultAsync(m => m.Username == username);
            }

            return user;
        }

        /// <summary>
        /// GetItemByEmailAsync
        /// </summary>
        /// <param name="email"></param>
        /// <param name="emailIsValid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<XM.UserInfo> GetItemByEmailAsync(string email, bool? emailIsValid, XM.UserStatus? status = null)
        {
            var query = _context.User.AsNoTracking().Where(m => m.Email == email);
            query.WhereIf(emailIsValid.HasValue, m => m.EmailIsValid == emailIsValid.Value);
            query.WhereIf(status.HasValue, m => m.Status == status.Value);
            var user = await query.Select(_selector).FirstOrDefaultAsync();
            return user;
        }

        /// <summary>
        /// GetItemByMobileAsync
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="mobileIsValid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<XM.UserInfo> GetItemByMobileAsync(string mobile, bool? mobileIsValid, XM.UserStatus? status = null)
        {
            var query = _context.User.AsNoTracking().Where(m => m.Mobile == mobile);
            query.WhereIf(mobileIsValid.HasValue, m => m.EmailIsValid == mobileIsValid.Value);
            query.WhereIf(status.HasValue, m => m.Status == status.Value);
            var user = await query.Select(_selector).FirstOrDefaultAsync();
            return user;
        }

        /// <summary>
        /// GetUserInfoWarpperListAsync
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public async Task<List<XM.UserInfoWarpper>> GetUserInfoWarpperListAsync(IEnumerable<int> userIds)
        {
            if (userIds == null) return new List<XM.UserInfoWarpper>(0);
            userIds = userIds.Distinct();
            var iDs = userIds as int[] ?? userIds.ToArray();
            var list = await _context.User.AsNoTracking().Where(m => iDs.Contains(m.UserId)).Select(m => new XM.UserInfoWarpper
            {
                UserId = m.UserId,
                Username = m.Username,
                DisplayName = m.DisplayName,
                AvatarUrl = m.AvatarUrl,
            }).AsNoTracking().ToListAsync();

            return list;
        }

        /// <summary>
        /// GetPageAsync
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public async Task<Page<XM.UserInfo>> GetPageAsync(XM.UserPageSearchCriteria criteria)
        {
            // 精简数据
            Expression<Func<User, XM.UserInfo>> selector = u => new XM.UserInfo
            {
                UserId = u.UserId,
                Username = u.Username,
                DisplayName = u.DisplayName,
                RealName = u.RealName,
                RealNameIsValid = u.RealNameIsValid,
                Email = u.Email,
                EmailIsValid = u.EmailIsValid,
                Mobile = u.Mobile,
                MobileIsValid = u.MobileIsValid,
                CreationTime = u.CreationTime,
                Description = u.Description,
                Status = u.Status,
                AvatarUrl = u.AvatarUrl,
                LogoUrl = u.LogoUrl,
                IsDeveloper = u.IsDeveloper,
                IsTester = u.IsTester,
                Group = new XM.GroupInfo
                {
                    GroupId = u.Group.GroupId,
                    Name = u.Group.Name,
                },
                Groups = from g in u.UserGroup
                         select new XM.GroupInfo
                         {
                             GroupId = g.GroupId,
                             Name = g.Group.Name
                         },
                Role = new XM.RoleInfo
                {
                    RoleId = u.Role != null ? u.Role.RoleId : Guid.Empty,
                    Name = u.Role != null ? u.Role.Name : String.Empty,
                },
                Roles = from r in u.UserRole
                        select new XM.RoleInfo
                        {
                            RoleId = r.RoleId,
                            Name = r.Role.Name,
                        },
                Permissions = from p in u.UserPermission
                              select new XM.PermissionBase
                              {
                                  PermissionId = p.PermissionId,
                                  Name = p.Permission.Name,
                                  ModuleName = p.Permission.ModuleName
                              }
            };

            IQueryable<User> query = _context.User;
            query = query.WhereIf(!criteria.GroupIds.IsNullOrEmpty(), m => criteria.GroupIds.Contains(m.GroupId));
            query = query.WhereIf(criteria.Status.HasValue, m => m.Status == criteria.Status.Value);
            if (criteria.CreationTimeBegin.HasValue)
            {
                var begin = criteria.CreationTimeBegin?.Date;
                query = query.Where(m => m.CreationTime >= begin);
            }
            if (criteria.CreationTimeEnd.HasValue)
            {
                var end = criteria.CreationTimeEnd?.Date.AddDays(1);
                query = query.Where(m => m.CreationTime < end);
            }
            if (criteria.Keyword != null)
            {
                var keyword = criteria.Keyword.Trim();
                if (keyword.Length != 0)
                {
                    query = query.Where(m =>
                        m.Username.Contains(keyword) ||
                        m.RealName.Contains(keyword) ||
                        m.Mobile.Contains(keyword) ||
                        m.DisplayName.Contains(keyword));
                }
            }

            IOrderedQueryable<User> orderedQuery;
            if (criteria.PagingInfo.SortInfo.IsValid())
            {
                orderedQuery = query.Order(criteria.PagingInfo.SortInfo);
            }
            else if (criteria.PagingInfo.SortInfos.IsValid())
            {
                orderedQuery = query.Order(criteria.PagingInfo.SortInfos);
            }
            else
            {
                // 默认排序
                orderedQuery = query.OrderBy(m => m.UserId);
            }

            var page = await orderedQuery.Select(selector).GetPageAsync(criteria.PagingInfo);
            return page;
        }

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<XM.UserInfo> SaveAsync(UserInput userInput, ModelStateDictionary modelState)
        {
            User userToSave;
            if (userInput is UserInputEdit userInputEdit)
            {
                userToSave = await _context.User.
                    Include(m => m.UserGroup).
                    Include(m => m.UserRole).
                    Include(m => m.UserPermission).
                    FirstOrDefaultAsync(m => m.UserId == userInputEdit.UserId);
                if (userToSave == null)
                {
                    modelState.AddModelError("UserId", "尝试编辑不存在的记录");
                    return null;
                }
                if (!userInput.Password.IsNullOrWhiteSpace())
                    userToSave.Password = userInput.Password;

                userToSave.Status = userInput.Status;

            }
            else
            {
                userToSave = new User();
                _context.User.Add(userToSave);
                userToSave.Status = XM.UserStatus.Normal; // Fix
                userToSave.Password = userInput.Password;
                userToSave.CreationTime = DateTime.Now;
            }

            var group = await _context.Group.Include(m => m.GroupAvailableRole).FirstOrDefaultAsync(m => m.GroupId == userInput.GroupId);
            if (group == null)
            {
                modelState.AddModelError("GroupId", "分组不存在");
                return null;
            }
            if (userInput.RoleId.HasValue && group.GroupAvailableRole.All(m => m.RoleId != userInput.RoleId.Value))
            {
                modelState.AddModelError("GroupId", $"分组【{group.Name}】不允许使用该角色");
                return null;
            }
            if (!group.IsContainsUser)
            {
                modelState.AddModelError("GroupId", $"分组【{group.Name}】不允许包含用户");
                return null;
            }

            userToSave.GroupId = userInput.GroupId;
            userToSave.RoleId = userInput.RoleId;
            userToSave.Username = userInput.Username;
            userToSave.DisplayName = userInput.DisplayName;
            userToSave.RealName = userInput.RealName;
            userToSave.RealNameIsValid = userInput.RealNameIsValid;
            userToSave.Email = userInput.Email;
            userToSave.EmailIsValid = userInput.EmailIsValid;
            userToSave.Mobile = userInput.Mobile;
            userToSave.MobileIsValid = userInput.MobileIsValid;
            userToSave.Description = userInput.Description;
            userToSave.IsDeveloper = userInput.IsDeveloper;
            userToSave.IsTester = userInput.IsTester;

            #region 分组

            //移除项
            if (!userToSave.UserGroup.IsNullOrEmpty())
            {
                if (!userInput.GroupIds.IsNullOrEmpty())
                {
                    List<UserGroup> groupToRemove = (from p in userToSave.UserGroup
                                                     where !userInput.GroupIds.Contains(p.GroupId)
                                                     select p).ToList();
                    for (int i = 0; i < groupToRemove.Count; i++)
                        userToSave.UserGroup.Remove(groupToRemove[i]);
                }
                else
                {
                    userToSave.UserGroup.Clear();
                }
            }
            //添加项
            if (!userInput.GroupIds.IsNullOrEmpty())
            {
                //要添加的Id集
                List<Guid> groupIdToAdd = (from p in userInput.GroupIds
                                           where userToSave.UserGroup.All(m => m.GroupId != p)
                                           select p).ToList();

                //要添加的项
                List<UserGroup> groupToAdd = await (from p in _context.Group
                                                    where groupIdToAdd.Contains(p.GroupId)
                                                    select new UserGroup
                                                    {
                                                        Group = p
                                                    }).ToListAsync();
                foreach (var item in groupToAdd)
                    userToSave.UserGroup.Add(item);
            }

            #endregion

            #region 用户角色

            //移除项
            if (!userToSave.UserRole.IsNullOrEmpty())
            {
                if (!userInput.RoleIds.IsNullOrEmpty())
                {
                    List<UserRole> roleToRemove = (from p in userToSave.UserRole
                                                   where !userInput.RoleIds.Contains(p.RoleId)
                                                   select p).ToList();
                    for (int i = 0; i < roleToRemove.Count; i++)
                        userToSave.UserRole.Remove(roleToRemove[i]);
                }
                else
                {
                    userToSave.UserRole.Clear();
                }
            }
            //添加项
            if (!userInput.RoleIds.IsNullOrEmpty())
            {
                //要添加的Id集
                List<Guid> roleIdToAdd = (from p in userInput.RoleIds
                                          where userToSave.UserRole.All(m => m.RoleId != p)
                                          select p).ToList();

                //要添加的项
                List<UserRole> roleToAdd = await (from p in _context.Role
                                                  where roleIdToAdd.Contains(p.RoleId)
                                                  select new UserRole
                                                  {
                                                      Role = p
                                                  }).ToListAsync();
                foreach (var item in roleToAdd)
                    userToSave.UserRole.Add(item);

            }

            #endregion

            #region 用户权限

            //移除项
            if (!userToSave.UserPermission.IsNullOrEmpty())
            {
                if (!userInput.PermissionIds.IsNullOrEmpty())
                {
                    List<UserPermission> permissionToRemove = (from p in userToSave.UserPermission
                                                               where !userInput.PermissionIds.Contains(p.PermissionId)
                                                               select p).ToList();
                    for (int i = 0; i < permissionToRemove.Count; i++)
                        userToSave.UserPermission.Remove(permissionToRemove[i]);
                }
                else
                {
                    userToSave.UserPermission.Clear();
                }
            }
            //添加项
            if (!userInput.PermissionIds.IsNullOrEmpty())
            {
                //要添加的Id集
                List<Guid> permissionIdToAdd = (from p in userInput.PermissionIds
                                                where userToSave.UserPermission.All(m => m.PermissionId != p)
                                                select p).ToList();

                //要添加的项
                List<UserPermission> permissionToAdd = await (from p in _context.Permission
                                                              where permissionIdToAdd.Contains(p.PermissionId)
                                                              select new UserPermission
                                                              {
                                                                  Permission = p
                                                              }).ToListAsync();
                foreach (var item in permissionToAdd)
                    userToSave.UserPermission.Add(item);

            }

            #endregion

            await _context.SaveChangesAsync();

            //return new[] { userToSave }.Select(_selector.Compile()).First();
            return await GetItemByUserIdAsync(userToSave.UserId);
        }

        /// <summary>
        /// ChangeUsernameAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newUsername"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangeUsernameAsync(int userId, string newUsername, ModelStateDictionary modelState)
        {
            var user = await _context.User.FirstOrDefaultAsync(m => m.UserId == userId);
            if (user == null)
            {
                modelState.AddModelError("UserId", "当前用户不存在");
                return false;
            }
            if (!user.Username.IsNullOrWhiteSpace() &&
                user.Username.Equals(newUsername, StringComparison.InvariantCultureIgnoreCase))
            {
                modelState.AddModelError("UserId", "目标用户名和当前用户名相同");
                return false;
            }
            if (_context.User.Any(m => m.UserId != userId && m.Username == newUsername))
            {
                modelState.AddModelError("UserId", $"用户名[{newUsername}]已经被使用");
                return false;
            }
            user.Username = newUsername;
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// ChangeDisplayNameAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="displayName"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangeDisplayNameAsync(int userId, string displayName, ModelStateDictionary modelState)
        {
            var user = await _context.User.FirstOrDefaultAsync(m => m.UserId == userId);
            if (user == null)
            {
                modelState.AddModelError("UserId", "当前用户不存在");
                return false;
            }
            user.DisplayName = displayName;
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// ChangePasswordAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangePasswordAsync(int userId, string password, ModelStateDictionary modelState)
        {
            var user = await _context.User.FirstOrDefaultAsync(m => m.UserId == userId);
            if (user == null)
            {
                modelState.AddModelError("Error", "用户不存在");
                return false;
            }

            user.Password = password;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// ChangePasswordAsync
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<int> ChangePasswordAsync(string username, string password, ModelStateDictionary modelState)
        {
            var user = await _context.User.FirstOrDefaultAsync(m => m.Username == username);
            if (user == null)
            {
                modelState.AddModelError("Error", "用户不存在");
                return 0;
            }
            user.Password = password;
            await _context.SaveChangesAsync();

            return user.UserId;
        }

        /// <summary>
        /// ChangeProfileAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userChangeProfileInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangeProfileAsync(int userId, UserChangeProfileInput userChangeProfileInput, ModelStateDictionary modelState)
        {
            var user = await _context.User.FirstOrDefaultAsync(m => m.UserId == userId);
            if (user == null)
            {
                modelState.AddModelError("Error", "用户不存在");
                return false;
            }
            user.DisplayName = userChangeProfileInput.DisplayName;
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// ResetPasswordByAccountAsync
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<int> ResetPasswordByAccountAsync(string account, string password, ModelStateDictionary modelState)
        {
            var user = await _context.User.FirstOrDefaultAsync(m => m.Username == account || (m.MobileIsValid && m.Mobile == account) || (m.EmailIsValid && m.Email == account));
            if (user == null)
            {
                modelState.AddModelError("Mobile", "重置密码失败:用户不存在");
                return 0;
            }
            user.Password = password;
            await _context.SaveChangesAsync();

            return user.UserId;
        }

        /// <summary>
        /// RemoveAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(int userId, ModelStateDictionary modelState)
        {
            User user = await _context.User.FirstOrDefaultAsync(m => m.UserId == userId);
            if (user == null)
                return false;
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    const string sql = "DELETE [NotificationUser] WHERE UserId = @UserId; " +
                                       "DELETE [Notification] WHERE FromUserId = @UserId OR ToUserId = @UserId;" +
                                       "DELETE UserGroup WHERE UserId = @UserId;" +
                                       "DELETE UserRole WHERE UserId = @UserId;" +
                                       "DELETE UserPermission WHERE UserId = @UserId;"
                        ;
                    await _context.Database.ExecuteSqlCommandAsync(sql, new SqlParameter("UserId", userId));

                    _context.User.Remove(user);
                    await _context.SaveChangesAsync();

                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    modelState.AddModelError("Exception", ex.Message);
                    return false;
                }

            }

            return true;
        }

        /// <summary>
        /// ChangeStatusAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangeStatusAsync(int userId, XM.UserStatus status, ModelStateDictionary modelState)
        {
            User user = await _context.User.FirstOrDefaultAsync(m => m.UserId == userId);
            if (user == null)
            {
                modelState.AddModelError("Error", "用户不存在");
                return false;
            }
            user.Status = status;
            await _context.SaveChangesAsync();

            return true;
        }

        #region UniqueId

        /// <summary>
        /// GetItemByUniqueIdAsync
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        public async Task<XM.UserInfo> GetItemByUniqueIdAsync(string uniqueId)
        {
            if (uniqueId.IsNullOrWhiteSpace()) return null;
            XM.UserInfo user = await _context.User.AsNoTracking().Where(m => m.UniqueId == uniqueId).Select(_selector).FirstOrDefaultAsync();
            return user;
        }

        /// <summary>
        /// GetOrGenerateItemByUniqueIdAsync
        /// </summary>
        /// <param name="generateGroupId"></param>
        /// <param name="generateStatus"></param>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        public async Task<XM.UserInfo> GetOrGenerateItemByUniqueIdAsync(Guid generateGroupId, XM.UserStatus generateStatus, string uniqueId)
        {
            if (uniqueId.IsNullOrWhiteSpace()) return null;
            var user = await GetItemByUniqueIdAsync(uniqueId);
            if (user == null)
            {
                var newUser = new User
                {
                    Status = generateStatus,
                    CreationTime = DateTime.Now,
                    UniqueId = uniqueId,
                    GroupId = generateGroupId, // new Guid("11111111-1111-1111-1111-111111111111") 等待分配组
                    Username = "U" + Guid.NewGuid().ToString("N").Substring(19),
                    Password = uniqueId,
                };

                _context.User.Add(newUser);
                await _context.SaveChangesAsync();
                user = await GetItemByUniqueIdAsync(uniqueId);
            }
            return user;
        }

        #endregion

        #region Verify exists

        /// <summary>
        /// IsExistsUsernameAsync
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsUsernameAsync(string username)
        {
            return await _context.User.AnyAsync(m => m.Username == username);
        }

        /// <summary>
        /// IsExistsEmailAsync
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsEmailAsync(string email)
        {
            if (email.IsNullOrWhiteSpace()) return false;
            return await _context.User.AnyAsync(m => m.Email == email);
        }

        /// <summary>
        /// IsExistsMobileAsync
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsMobileAsync(string mobile)
        {
            if (mobile.IsNullOrWhiteSpace()) return false;
            return await _context.User.AnyAsync(m => m.Mobile == mobile);
        }

        /// <summary>
        /// IsExistsAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(int userId, XM.UserStatus? status = null)
        {
            var query = _context.User.Where(m => m.UserId == userId);
            query = query.WhereIf(status.HasValue, m => m.Status == status);
            return await query.AnyAsync();
        }

        /// <summary>
        /// VerifyExistsUsernameAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<bool> VerifyExistsUsernameAsync(int userId, string username)
        {
            if (username.IsNullOrWhiteSpace()) return false;
            return await _context.User.AnyAsync(m => m.UserId != userId && m.Username == username);
        }

        /// <summary>
        /// VerifyExistsUsernameAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public async Task<bool> VerifyExistsMobileAsync(int userId, string mobile)
        {
            if (mobile.IsNullOrWhiteSpace()) return false;
            return await _context.User.AnyAsync(m => m.UserId != userId && m.Mobile == mobile);
        }

        /// <summary>
        /// VerifyExistsEmailAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> VerifyExistsEmailAsync(int userId, string email)
        {
            if (email.IsNullOrWhiteSpace()) return false;
            return await _context.User.AnyAsync(m => m.UserId != userId && m.Email == email);
        }

        /// <summary>
        /// VerifyExistsAsync
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> VerifyExistsAsync(UserInput userInput, ModelStateDictionary modelState)
        {
            var username = userInput.Username;
            var mobile = userInput.Mobile.IsNullOrWhiteSpace() ? null : userInput.Mobile;
            var email = userInput.Email.IsNullOrWhiteSpace() ? null : userInput.Email;

            bool isExistsUsername = false;
            bool isExistsMobile = false;
            bool isExistsEmail = false;
            var item = new
            {
                Username = String.Empty,
                Email = String.Empty,
                Mobile = String.Empty,
            };
            if (userInput is UserInputEdit userInputEdit) //根据用户 Id 编辑
            {

                item = await _context.User.AsNoTracking().Where(m => m.UserId != userInputEdit.UserId &&
                (m.Username == username ||
                (mobile != null && m.Mobile == userInput.Mobile) ||
                (email != null && m.Email == userInput.Email))).Select(m => new
                {
                    m.Username,
                    m.Email,
                    m.Mobile,
                }).FirstOrDefaultAsync();
            }
            else //添加
            {
                item = await _context.User.AsNoTracking().Where(m => m.Username == username ||
                (mobile != null && m.Mobile == userInput.Mobile) ||
                (email != null && m.Email == userInput.Email)).Select(m => new
                {
                    m.Username,
                    m.Email,
                    m.Mobile,
                }).FirstOrDefaultAsync();

            }

            if (item != null)
            {
                if (!item.Username.IsNullOrWhiteSpace() && item.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase))
                {
                    isExistsUsername = true;
                }
                else if (!item.Mobile.IsNullOrWhiteSpace() && item.Mobile.Equals(mobile, StringComparison.InvariantCultureIgnoreCase))
                {
                    isExistsMobile = true;
                }
                else if (!item.Email.IsNullOrWhiteSpace() && item.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase))
                {
                    isExistsEmail = true;
                }

                if (isExistsUsername)
                {
                    modelState.AddModelError("Username", "用户名[" + username + "]已经被使用");
                }
                else if (isExistsMobile)
                {
                    modelState.AddModelError("Mobile", "手机号[" + mobile + "]已经被使用");

                }
                else if (isExistsEmail)
                {
                    modelState.AddModelError("Mobile", "邮箱[" + email + "]已经被使用");
                }
            }

            return isExistsUsername || isExistsMobile || isExistsEmail;
        }

        #endregion

        #region Avatar & Logo

        /// <summary>
        /// GetAvatarUrlAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetAvatarUrlAsync(int userId)
        {
            var head = await _context.User.AsNoTracking().Where(m => m.UserId == userId).Select(m => m.AvatarUrl).FirstOrDefaultAsync();
            return head;
        }

        /// <summary>
        /// GetLogoUrlAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<string> GetLogoUrlAsync(int userId)
        {
            var head = await _context.User.AsNoTracking().Where(m => m.UserId == userId).Select(m => m.LogoUrl).FirstOrDefaultAsync();
            return head;
        }

        /// <summary>
        /// ChangeHeadAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="avatarUrl"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangeAvatarAsync(int userId, string avatarUrl, ModelStateDictionary modelState)
        {
            var user = await _context.User.FirstOrDefaultAsync(m => m.UserId == userId);
            if (user == null)
            {
                modelState.AddModelError("Error", "用户不存在");
                return false;
            }

            user.AvatarUrl = avatarUrl;
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// ChangeLogoAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="logoUrl"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> ChangeLogoAsync(int userId, string logoUrl, ModelStateDictionary modelState)
        {
            var user = await _context.User.FirstOrDefaultAsync(m => m.UserId == userId);
            if (user == null)
            {
                modelState.AddModelError("Error", "用户不存在");
                return false;
            }
            user.LogoUrl = logoUrl;
            await _context.SaveChangesAsync();

            return true;
        }

        #endregion
    }
}
