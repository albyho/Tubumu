using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Tubumu.Modules.Admin.Repositories.Entities;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;
using XM = Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.Repositories
{
    /// <summary>
    /// IRoleRepository
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<XM.Role> GetItemAsync(Guid roleId);

        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<XM.Role> GetItemAsync(string name);

        /// <summary>
        /// GetBaseListAsync
        /// </summary>
        /// <returns></returns>
        Task<List<XM.RoleBase>> GetBaseListAsync();

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <returns></returns>
        Task<List<XM.Role>> GetListAsync();

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="roleInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> SaveAsync(RoleInput roleInput, ModelStateDictionary modelState);

        /// <summary>
        /// RemoveAsync
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(Guid roleId, ModelStateDictionary modelState);

        /// <summary>
        /// SaveNameAsync
        /// </summary>
        /// <param name="saveRoleNameInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> SaveNameAsync(RoleNameInput saveRoleNameInput, ModelStateDictionary modelState);

        /// <summary>
        /// MoveAsync
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        Task<bool> MoveAsync(Guid roleId, MovingTarget target);

        /// <summary>
        /// MoveAsync
        /// </summary>
        /// <param name="sourceDisplayOrder"></param>
        /// <param name="targetDisplayOrder"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> MoveAsync(int sourceDisplayOrder, int targetDisplayOrder, ModelStateDictionary modelState);

        /// <summary>
        /// MoveAsync
        /// </summary>
        /// <param name="sourceRoleId"></param>
        /// <param name="targetRoleId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> MoveAsync(Guid sourceRoleId, Guid targetRoleId, ModelStateDictionary modelState);
    }

    /// <summary>
    /// RoleRepository
    /// </summary>
    public class RoleRepository : IRoleRepository
    {
        private readonly TubumuContext _context;
        private readonly Expression<Func<Role, XM.Role>> _selector;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public RoleRepository(TubumuContext context)
        {
            _context = context;

            _selector = r => new XM.Role
            {
                RoleId = r.RoleId,
                Name = r.Name,
                IsSystem = r.IsSystem,
                DisplayOrder = r.DisplayOrder,
                Permissions = from p in r.RolePermission
                              orderby p.Permission.DisplayOrder
                              select new XM.PermissionBase
                              {
                                  ModuleName = p.Permission.ModuleName,
                                  PermissionId = p.PermissionId,
                                  Name = p.Permission.Name
                              }
            };
        }

        #region IRoleRepository 成员

        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<XM.Role> GetItemAsync(Guid roleId)
        {
            return await _context.Role.AsNoTracking().Select(_selector).FirstOrDefaultAsync(m => m.RoleId == roleId);
        }

        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<XM.Role> GetItemAsync(string name)
        {
            return await _context.Role.AsNoTracking().Select(_selector).FirstOrDefaultAsync(m => m.Name == name);
        }

        /// <summary>
        /// GetBaseListAsync
        /// </summary>
        /// <returns></returns>
        public async Task<List<XM.RoleBase>> GetBaseListAsync()
        {
            return await (from r in _context.Role.AsNoTracking()
                          orderby r.DisplayOrder
                          select new XM.RoleBase
                          {
                              RoleId = r.RoleId,
                              Name = r.Name,
                              IsSystem = r.IsSystem,
                              DisplayOrder = r.DisplayOrder
                          }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <returns></returns>
        public async Task<List<XM.Role>> GetListAsync()
        {
            return await _context.Role.AsNoTracking().OrderBy(m => m.DisplayOrder).Select(_selector).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="roleInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> SaveAsync(RoleInput roleInput, ModelStateDictionary modelState)
        {
            Role roleToSave = null;
            if (roleInput.RoleId.HasValue)
            {
                roleToSave = await _context.Role.
                    Include(m=>m.RolePermission).
                    FirstOrDefaultAsync(m => m.RoleId == roleInput.RoleId.Value);
            }
            if (roleToSave == null)
            {
                roleToSave = new Role
                {
                    RoleId = Guid.NewGuid(),
                    IsSystem = false
                };
                _context.Role.Add(roleToSave);
                int maxDisplayOrder = await _context.Role.MaxAsync(m => (int?)m.DisplayOrder) ?? 0;
                roleToSave.DisplayOrder = maxDisplayOrder + 1;
            }
            roleToSave.Name = roleInput.Name;

            #region 角色权限
            if (!roleToSave.RolePermission.IsNullOrEmpty())
            {
                // 移除项
                if (!roleInput.PermissionIds.IsNullOrEmpty())
                {
                    List<RolePermission> permissionToRemove = (from p in roleToSave.RolePermission
                                                           where !roleInput.PermissionIds.Contains(p.PermissionId)
                                                           select p).ToList();
                    for (int i = 0; i < permissionToRemove.Count; i++)
                        roleToSave.RolePermission.Remove(permissionToRemove[i]);
                }
                else
                {
                    roleToSave.RolePermission.Clear();
                }
            }
            if (!roleInput.PermissionIds.IsNullOrEmpty())
            {            
                // 添加项
                // 要添加的Id集
                List<Guid> permissionIdToAdd = (from p in roleInput.PermissionIds
                                                where roleToSave.RolePermission.All(m => m.PermissionId != p)
                                                select p).ToList();

                // 要添加的项
                List<RolePermission> permissionToAdd = await (from p in _context.Permission
                                                          where permissionIdToAdd.Contains(p.PermissionId)
                                                          select new RolePermission
                                                          {
                                                              Permission = p
                                                          }).ToListAsync();
                foreach (var item in permissionToAdd)
                    roleToSave.RolePermission.Add(item);

            }
            #endregion

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// RemoveAsync
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(Guid roleId, ModelStateDictionary modelState)
        {
            var roleToRemove = await _context.Role.FirstOrDefaultAsync(m => m.RoleId == roleId);
            if (roleToRemove == null || roleToRemove.IsSystem)
            {
                modelState.AddModelError("RoleId", "记录不存在");
                return false;
            }

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                var sql = "Update Role Set DisplayOrder=DisplayOrder-1 Where DisplayOrder>@DisplayOrder";
                await _context.Database.ExecuteSqlCommandAsync(sql, new SqlParameter("DisplayOrder", roleToRemove.DisplayOrder));

                // Delete GroupAvailableRole Where RoleId=@RoleId
                sql = "Delete RolePermission Where RoleId=@RoleId; Delete GroupRole Where RoleId=@RoleId; UPDATE [User] SET RoleId = null WHERE RoleId=@RoleId;";
                await _context.Database.ExecuteSqlCommandAsync(sql, new SqlParameter("RoleId", roleId));

                _context.Role.Remove(roleToRemove);

                await _context.SaveChangesAsync();
                dbContextTransaction.Commit();
            }

            return true;
        }

        /// <summary>
        /// SaveNameAsync
        /// </summary>
        /// <param name="saveRoleNameInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> SaveNameAsync(RoleNameInput saveRoleNameInput, ModelStateDictionary modelState)
        {
            var roleToRemove = await _context.Role.FirstOrDefaultAsync(m => m.RoleId == saveRoleNameInput.RoleId);
            if (roleToRemove == null || roleToRemove.IsSystem)
            {
                modelState.AddModelError("RoleId", "记录不存在");
                return false;
            }

            if (saveRoleNameInput.Name != roleToRemove.Name)
            {
                roleToRemove.Name = saveRoleNameInput.Name;
                await _context.SaveChangesAsync();
            }

            return true;
        }

        /// <summary>
        /// MoveAsync
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public async Task<bool> MoveAsync(Guid roleId, MovingTarget target)
        {
            var roleToMove = await _context.Role.FirstOrDefaultAsync(m => m.RoleId == roleId);

            // 保证DisplayOrder为 1 的“系统管理员”不被移动
            if (roleToMove == null || roleToMove.DisplayOrder == 1) return false;

            // 防止DisplayOrder为2的项非法篡改"系统管理员",也就是说DisplayOrder必须大于2
            if (MovingTarget.Up == target)
            {
                if (roleToMove.DisplayOrder < 3) return false;

                var targetRole = await _context.Role.OrderByDescending(m => m.DisplayOrder).FirstOrDefaultAsync(m => m.DisplayOrder < roleToMove.DisplayOrder);
                // 某种原因导致当前项之前已经没有项了
                if (targetRole == null) return false;

                roleToMove.DisplayOrder--;
                targetRole.DisplayOrder++;
                await _context.SaveChangesAsync();

            }
            else if (MovingTarget.Down == target)
            {
                var targetRole = await _context.Role.OrderBy(m => m.DisplayOrder).FirstOrDefaultAsync(m => m.DisplayOrder > roleToMove.DisplayOrder);
                // 某种原因导致当前项之后已经没有项了
                if (targetRole == null) return false;

                roleToMove.DisplayOrder++;
                targetRole.DisplayOrder--;
                await _context.SaveChangesAsync();
            }
            return true;
        }

        /// <summary>
        /// MoveAsync
        /// </summary>
        /// <param name="sourceDisplayOrder"></param>
        /// <param name="targetDisplayOrder"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> MoveAsync(int sourceDisplayOrder, int targetDisplayOrder, ModelStateDictionary modelState)
        {
            if (sourceDisplayOrder == targetDisplayOrder)
            {
                modelState.AddModelError("SourceDisplayOrder", "源DisplayOrder和目标DisplayOrder不能相同");
                return false;
            }
            var sourceRole = await _context.Role.FirstOrDefaultAsync(m => m.DisplayOrder == sourceDisplayOrder);
            if (sourceRole == null)
            {
                modelState.AddModelError("SourceDisplayOrder", "源记录不存在");
                return false;
            }
            var targetRole = await _context.Role.FirstOrDefaultAsync(m => m.DisplayOrder == targetDisplayOrder);
            if (targetRole == null)
            {
                modelState.AddModelError("TargetDisplayOrder", "目标记录不存在");
                return false;
            }
            return await MoveAsync(sourceRole, targetRole, modelState);
        }

        /// <summary>
        /// MoveAsync
        /// </summary>
        /// <param name="sourceRoleId"></param>
        /// <param name="targetRoleId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> MoveAsync(Guid sourceRoleId, Guid targetRoleId, ModelStateDictionary modelState)
        {
            if (sourceRoleId == targetRoleId)
            {
                modelState.AddModelError("SourceGroupId", "源Id和目标Id不能相同");
                return false;
            }
            var sourceRole = await _context.Role.FirstOrDefaultAsync(m => m.RoleId == sourceRoleId);
            if (sourceRole == null)
            {
                modelState.AddModelError("SourceGroupId", "源记录不存在");
                return false;
            }
            var targetRole = await _context.Role.FirstOrDefaultAsync(m => m.RoleId == targetRoleId);
            if (targetRole == null)
            {
                modelState.AddModelError("TargetGroupId", "目标记录不存在");
                return false;
            }

            return await MoveAsync(sourceRole, targetRole, modelState);
        }

        /// <summary>
        /// MoveAsync
        /// </summary>
        /// <param name="sourceRole"></param>
        /// <param name="targetRole"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        private async Task<bool> MoveAsync(Role sourceRole, Role targetRole, ModelStateDictionary modelState)
        {
            if (sourceRole.DisplayOrder == targetRole.DisplayOrder)
            {
                modelState.AddModelError("SourceGroupId", "源DisplayOrder和目标DisplayOrder不能相同");
                return false;
            }

            // 不允许移动系统管理员
            if (sourceRole.DisplayOrder == 1)
            {
                modelState.AddModelError("SourceGroupId", "不允许移动系统管理员");
                return false;
            }

            // 不允许移动到系统管理员之前
            if (targetRole.DisplayOrder == 1)
            {
                modelState.AddModelError("SourceGroupId", "不允许移动到系统管理员之前");
                return false;
            }

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                string sql;
                if (sourceRole.DisplayOrder > targetRole.DisplayOrder)
                {
                    // 向上移动。目标节点及以下，至，源节点之间的节点，序号 + 1
                    sql = "Update Role Set DisplayOrder = DisplayOrder + 1 Where DisplayOrder >= @TargetDisplayOrder And DisplayOrder < @SourceDisplayOrder;";
                }
                else
                {
                    // 向下移动。目标节点及以上，至，源节点之间的节点，序号 - 1
                    sql = "Update Role Set DisplayOrder = DisplayOrder - 1 Where DisplayOrder <= @TargetDisplayOrder And DisplayOrder > @SourceDisplayOrder;";
                }

                await _context.Database.ExecuteSqlCommandAsync(sql,
                    new SqlParameter("SourceDisplayOrder", sourceRole.DisplayOrder),
                    new SqlParameter("TargetDisplayOrder", targetRole.DisplayOrder));

                sourceRole.DisplayOrder = targetRole.DisplayOrder;

                await _context.SaveChangesAsync();
                dbContextTransaction.Commit();
            }

            return true;
        }

        #endregion
    }
}
