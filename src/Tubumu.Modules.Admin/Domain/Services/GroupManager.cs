using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Tubumu.Modules.Admin.Domain.Entities;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;
using XM = Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.Domain.Services
{
    /// <summary>
    /// IGroupManager
    /// </summary>
    public interface IGroupManager
    {
        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<XM.Group> GetItemAsync(Guid groupId);

        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<XM.Group> GetItemAsync(string name);

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<List<XM.Group>> GetListAsync(Guid? parentId = null);

        /// <summary>
        /// GetBasePathAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<List<XM.GroupBase>> GetBasePathAsync(Guid groupId);

        /// <summary>
        /// GetInfoPathAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<List<XM.GroupInfo>> GetInfoPathAsync(Guid groupId);

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="groupInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> SaveAsync(GroupInput groupInput, ModelStateDictionary modelState);

        /// <summary>
        /// RemoveAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(Guid groupId, ModelStateDictionary modelState);

        /// <summary>
        /// MoveAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="movingTarget"></param>
        /// <returns></returns>
        Task<bool> MoveAsync(Guid groupId, MovingTarget movingTarget);

        /// <summary>
        /// MoveAsync
        /// </summary>
        /// <param name="sourceGroupId"></param>
        /// <param name="targetGroupId"></param>
        /// <param name="movingLocation"></param>
        /// <param name="isChild"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> MoveAsync(Guid sourceGroupId, Guid targetGroupId, MovingLocation movingLocation, bool? isChild, ModelStateDictionary modelState);

        /// <summary>
        /// MoveByDisplayOrderAsync
        /// </summary>
        /// <param name="sourceDisplayOrder"></param>
        /// <param name="targetDisplayOrder"></param>
        /// <param name="movingLocation"></param>
        /// <param name="isChild"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> MoveByDisplayOrderAsync(int sourceDisplayOrder, int targetDisplayOrder, MovingLocation movingLocation, bool? isChild, ModelStateDictionary modelState);
    }

    /// <summary>
    /// GroupManager
    /// </summary>
    public class GroupManager : IGroupManager
    {
        private readonly TubumuContext _context;
        private readonly Expression<Func<Group, XM.Group>> _selector;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public GroupManager(TubumuContext context)
        {
            _context = context;

            _selector = ug => new XM.Group
            {
                GroupId = ug.GroupId,
                Name = ug.Name,
                IsContainsUser = ug.IsContainsUser,
                IsDisabled = ug.IsDisabled,
                IsSystem = ug.IsSystem,
                DisplayOrder = ug.DisplayOrder,
                ParentId = ug.ParentId,
                Level = ug.Level,
                Roles = from r in ug.GroupRole
                        orderby r.Role.DisplayOrder
                        select new XM.RoleBase
                        {
                            RoleId = r.RoleId,
                            Name = r.Role.Name,
                            IsSystem = r.Role.IsSystem,
                            DisplayOrder = r.Role.DisplayOrder
                        },
                AvailableRoles = from r in ug.GroupAvailableRole
                                 orderby r.Role.DisplayOrder
                                 select new XM.RoleBase
                                 {
                                     RoleId = r.RoleId,
                                     Name = r.Role.Name,
                                     IsSystem = r.Role.IsSystem,
                                     DisplayOrder = r.Role.DisplayOrder
                                 },
                Permissions = from p in ug.GroupPermission
                              orderby p.Permission.DisplayOrder
                              select new XM.PermissionBase
                              {
                                  PermissionId = p.PermissionId,
                                  ModuleName = p.Permission.ModuleName,
                                  Name = p.Permission.Name,
                              }
            };
        }

        #region IGroupManager 成员

        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<XM.Group> GetItemAsync(Guid groupId)
        {
            return await _context.Group.AsNoTracking().Where(m => m.GroupId == groupId).Select(_selector).FirstOrDefaultAsync();
        }

        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<XM.Group> GetItemAsync(string name)
        {
            return await _context.Group.AsNoTracking().Where(m => m.Name == name).Select(_selector).FirstOrDefaultAsync();
        }

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<List<XM.Group>> GetListAsync(Guid? parentId = null)
        {
            if (parentId.HasValue)
            {
                var parent = await _context.Group.AsNoTracking().FirstOrDefaultAsync(m => m.GroupId == parentId.Value);
                if (parent == null)
                    return new List<XM.Group>();
                else
                {
                    int displayOrderOfNextParentOrNextBrother = await GetDisplayOrderOfNextParentOrNextBrotherAsync(parent);
                    if (displayOrderOfNextParentOrNextBrother != 0)
                        return await _context.Group.AsNoTracking().Where(m => m.DisplayOrder >= parent.DisplayOrder && m.DisplayOrder < displayOrderOfNextParentOrNextBrother)
                            .OrderBy(m => m.DisplayOrder)
                            .Select(_selector)
                            .AsNoTracking()
                            .ToListAsync();
                    else
                        return await _context.Group.AsNoTracking().Where(m => m.DisplayOrder >= parent.DisplayOrder)
                            .OrderBy(m => m.DisplayOrder)
                            .Select(_selector)
                            .AsNoTracking()
                            .ToListAsync();
                }
            }
            else
            {
                return await _context.Group
                    .AsNoTracking()
                    .OrderBy(m => m.DisplayOrder)
                    .Select(_selector)
                    .AsNoTracking()
                    .ToListAsync();
            }
        }

        /// <summary>
        /// GetBasePathAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<List<XM.GroupBase>> GetBasePathAsync(Guid groupId)
        {
            const string sql = @"WITH CET AS
                     (
                     SELECT GroupId,Name,DisplayOrder,ParentId,IsContainsUser,IsSystem
                     FROM [Group]
                     WHERE  GroupId = @GroupId
                     UNION ALL
                     SELECT P.GroupId,P.Name,P.DisplayOrder,P.ParentId,P.IsContainsUser,P.IsSystem
                     FROM [Group] P
                     JOIN CET Curr ON Curr.ParentId = P.GroupId
                    )
                    SELECT GroupId,Name,DisplayOrder,ParentId,IsContainsUser,IsSystem 
                    FROM CET ORDER BY DisplayOrder";

            return await _context.Group.FromSql(sql, new SqlParameter("GroupId", groupId)).Select(m => new XM.GroupBase
            {
                GroupId = m.GroupId,
                Name = m.Name,
                IsContainsUser = m.IsContainsUser,
                IsDisabled = m.IsDisabled,
                IsSystem = m.IsSystem,
            }).ToListAsync();
        }

        /// <summary>
        /// GetInfoPathAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<List<XM.GroupInfo>> GetInfoPathAsync(Guid groupId)
        {
            const string sql = @"WITH CET AS
                     (
                     SELECT GroupId,Name,ParentId,DisplayOrder
                     FROM [Group]
                     WHERE  GroupId = @GroupId
                     UNION ALL
                     SELECT P.GroupId,P.Name,P.ParentId
                     FROM [Group] P
                     JOIN CET Curr ON Curr.ParentId = P.GroupId
                    )
                    SELECT GroupId,Name,DisplayOrder,ParentId
                    FROM CET ORDER BY DisplayOrder";

            return await _context.Group.FromSql(sql, new SqlParameter("GroupId", groupId)).Select(m => new XM.GroupInfo
            {
                GroupId = m.GroupId,
                Name = m.Name,
            }).ToListAsync();
        }

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="groupInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> SaveAsync(GroupInput groupInput, ModelStateDictionary modelState)
        {
            string sql;
            Group groupToSave = null;
            Group parent = null;
            if (groupInput.GroupId == groupInput.ParentId)
            {
                modelState.AddModelError("GroupId", "尝试将自身作为父节点");
                return false;
            }

            if (!groupInput.ParentId.IsNullOrEmpty())
            {
                parent = await _context.Group.AsNoTracking().FirstOrDefaultAsync(m => m.GroupId == groupInput.ParentId.Value);
                if (parent == null)
                {
                    modelState.AddModelError("GroupId", "尝试添加或编辑至不存在的父节点上");
                    return false;
                }
                if (parent.IsSystem)
                {
                    modelState.AddModelError("GroupId", "不允许将节点添加至系统节点上");
                    return false;
                }
            }

            if (!groupInput.GroupId.IsNullOrEmpty())
            {
                groupToSave = await _context.Group.
                    Include(m => m.GroupAvailableRole).
                    Include(m => m.GroupRole).
                    Include(m => m.GroupPermission).
                    FirstOrDefaultAsync(m => m.GroupId == groupInput.GroupId.Value);
            }

            // 添加操作
            if (groupToSave == null)
            {
                #region 添加操作
                // 创建要保存的对象
                groupToSave = new Group
                {
                    GroupId = Guid.NewGuid(),
                    ParentId = groupInput.ParentId,
                    IsSystem = false,
                };
                _context.Group.Add(groupToSave);
                if (parent == null)
                {
                    // 如果添加的是新的顶级节点,直接添加到末尾，不会影响其他节点
                    groupToSave.DisplayOrder = await GetMaxDisplayOrderAsync() + 1;
                    groupToSave.Level = 1;
                }
                else
                {
                    //如果添加的是子节点，会影响其他节点的DisplayOrder

                    //父节点树的最大DisplayerOrder
                    int maxDisplayOrderInParentTree = await GetMaxDisplayOrderInTreeAsync(groupInput.ParentId.Value);
                    //父节点树的最大DisplayerOrder基础上加1作为保存对象的DisplayOrder
                    groupToSave.DisplayOrder = maxDisplayOrderInParentTree + 1;
                    //父节点的Level基础上加1作为保存对象的Level
                    groupToSave.Level = parent.Level + 1;

                    //父节点树之后的所有节点的DisplayOrder加1
                    sql = "Update [Group] Set DisplayOrder = DisplayOrder + 1 Where DisplayOrder > @DisplayOrder";
                    await _context.Database.ExecuteSqlCommandAsync(sql, new SqlParameter("DisplayOrder", maxDisplayOrderInParentTree));
                }

                #endregion
            }
            else if (groupInput.ParentId != groupToSave.ParentId)
            {
                //如果父节点不改变，则仅仅保存数据就行了。下面处理的是父节点改变了的情况
                //如果父节点改变(从无父节点到有父节点，从有父节点到无父节点，从一个父节点到另一个父节点)
                groupToSave.ParentId = groupInput.ParentId;

                //获取当前节点的下一个兄弟节点或更高层下一个父节点 (不是自己的父节点)的DisplayOrder
                int displayOrderOfNextParentOrNextBrother = await GetDisplayOrderOfNextParentOrNextBrotherAsync(groupToSave);
                // 当前节点树Id集合
                var currTreeIds = await GetTreeIdListAsync(groupToSave, displayOrderOfNextParentOrNextBrother, true);
                int currentTreeItemCount = currTreeIds.Count;

                if (!groupToSave.ParentId.HasValue)
                {
                    //当前节点将由子节点升为顶级节点，直接将该节点数移到最后

                    #region 将由子节点升为顶级节点 (成为最后一个顶级节点)

                    //需要提升的层级数
                    int xLevel = groupToSave.Level - 1;

                    if (displayOrderOfNextParentOrNextBrother == 0)
                    {
                        //当前节点树之后已无任何节点
                        //将当前节点树的所有节点的Level都进行提升
                        sql = "Update [Group] Set Level = Level - @Level Where DisplayOrder>=@DisplayOrder";
                        await _context.Database.ExecuteSqlCommandAsync(sql
                            , new SqlParameter("Level", xLevel)
                            , new SqlParameter("DisplayOrder", groupToSave.DisplayOrder)
                            );
                    }
                    else
                    {
                        //当前节点树之后还有节点，应该将这些节点的向前面排，并且将当前节点树的所有节点往后排
                        //当前节点树之后的节点数量
                        int nextItemCount = await _context.Group.CountAsync(m => m.DisplayOrder >= displayOrderOfNextParentOrNextBrother);

                        sql = "Update [Group] Set DisplayOrder = DisplayOrder - @CTIC Where DisplayOrder>=@DOONPONB";

                        await _context.Database.ExecuteSqlCommandAsync(sql
                            , new SqlParameter("CTIC", currentTreeItemCount)
                            , new SqlParameter("DOONPONB", displayOrderOfNextParentOrNextBrother)
                            );

                        sql = "Update [Group] Set Level = Level - @Level,DisplayOrder = DisplayOrder + @NextItemCount Where 1<>1 ";
                        foreach (var id in currTreeIds)
                            sql += $" Or GroupId = '{id}'";

                        await _context.Database.ExecuteSqlCommandAsync(sql
                            , new SqlParameter("Level", xLevel)
                            , new SqlParameter("NextItemCount", nextItemCount)
                            );

                    }

                    #endregion
                }
                else
                {
                    //当前节点将改变父节点，包括从顶级节点移至另一节点下，或从当前父节点下移至另一节点下

                    #region 从顶级节点移至另一节点下，或从当前父节点下移至另一节点下 (成为目标节点的最有一个子节点)

                    //目标父节点
                    var newParent = await _context.Group.AsNoTracking().FirstOrDefaultAsync(m => m.GroupId == groupInput.ParentId.Value);

                    int xDisplayOrder = groupToSave.DisplayOrder - newParent.DisplayOrder;
                    int xLevel = groupToSave.Level - newParent.Level;

                    if (xDisplayOrder > 0) //从下往上移
                    {
                        #region 从下往上移

                        //特例处理，当前节点要移至的父节点就是上一个节点，只需要改变当前树Level
                        if (xDisplayOrder == 1)
                        {
                            sql = "Update [Group] Set Level = Level - @Level Where 1<>1 ";
                            foreach (var id in currTreeIds)
                                sql += $" Or GroupId = '{id}'";

                            await _context.Database.ExecuteSqlCommandAsync(sql
                                , new SqlParameter("Level", xLevel - 1)
                                );
                        }
                        else
                        {
                            //新的父节点和本节点之间的节点往下移动，DisplayOrder增加
                            sql = "Update [Group] Set DisplayOrder=DisplayOrder+@CurTreeCount Where DisplayOrder>@TDisplayOrder And DisplayOrder<@CDisplayOrder";
                            await _context.Database.ExecuteSqlCommandAsync(sql
                                , new SqlParameter("CurTreeCount", currentTreeItemCount)
                                , new SqlParameter("TDisplayOrder", newParent.DisplayOrder)
                                , new SqlParameter("CDisplayOrder", groupToSave.DisplayOrder)
                                );

                            sql = "Update [Group] Set DisplayOrder = DisplayOrder-@XCount,Level = Level - @Level Where 1<>1 ";
                            foreach (var id in currTreeIds)
                                sql += $" Or GroupId = '{id}'";
                            await _context.Database.ExecuteSqlCommandAsync(sql
                                , new SqlParameter("XCount", xDisplayOrder - 1)//也就是新节点和本节点之间的节点的数量
                                , new SqlParameter("Level", xLevel - 1)
                                );

                        }

                        #endregion
                    }
                    else//从上往下移
                    {
                        #region 从上往下移

                        // 本节点树下已经不存在任何节点了，当然无法向下移
                        if (displayOrderOfNextParentOrNextBrother == 0)
                        {
                            modelState.AddModelError("GroupId", "无法下移");
                            return false;
                        }

                        // 更新本节点树至新的父节点 (包括新的父节点)之间的节点的DisplayOrder
                        sql = "Update [Group] Set DisplayOrder=DisplayOrder-@CurTreeCount Where DisplayOrder>=@DOONPONB And DisplayOrder<=@TDisplayOrder";
                        await _context.Database.ExecuteSqlCommandAsync(sql
                        , new SqlParameter("CurTreeCount", currentTreeItemCount)
                            , new SqlParameter("DOONPONB", displayOrderOfNextParentOrNextBrother)
                            , new SqlParameter("TDisplayOrder", newParent.DisplayOrder)
                            );

                        // 本节点至新的节点之间的节点数
                        int nextItemCount = newParent.DisplayOrder - displayOrderOfNextParentOrNextBrother + 1;
                        sql = "Update [Group] Set DisplayOrder = DisplayOrder+ @XCount,Level = Level - @Level Where 1<>1 ";
                        foreach (var id in currTreeIds)
                            sql += $" Or GroupId = '{id}'";
                        await _context.Database.ExecuteSqlCommandAsync(sql
                        , new SqlParameter("XCount", nextItemCount)
                            , new SqlParameter("Level", xLevel - 1)
                            );

                        #endregion
                    }

                    #endregion
                }
            }

            groupToSave.Name = groupInput.Name;
            groupToSave.IsContainsUser = groupInput.IsContainsUser;
            groupToSave.IsDisabled = groupInput.IsDisabled;

            #region 角色

            // 移除项
            if (!groupToSave.GroupRole.IsNullOrEmpty())
            {
                if (!groupInput.RoleIds.IsNullOrEmpty())
                {
                    List<GroupRole> roleToRemove = (from p in groupToSave.GroupRole
                                                    where !groupInput.RoleIds.Contains(p.RoleId)
                                                    select p).ToList();
                    for (int i = 0; i < roleToRemove.Count; i++)
                        groupToSave.GroupRole.Remove(roleToRemove[i]);
                }
                else
                    groupToSave.GroupRole.Clear();
            }
            // 添加项
            if (!groupInput.RoleIds.IsNullOrEmpty())
            {
                // 要添加的Id集
                List<Guid> roleIdToAdd = (from p in groupInput.RoleIds
                                          where groupToSave.GroupRole.All(m => m.RoleId != p)
                                          select p).ToList();

                // 要添加的项
                List<GroupRole> roleToAdd = await (from p in _context.Role
                                                   where roleIdToAdd.Contains(p.RoleId)
                                                   select new GroupRole
                                                   {
                                                       Role = p
                                                   }).ToListAsync();
                foreach (var item in roleToAdd)
                    groupToSave.GroupRole.Add(item);

            }

            #endregion

            #region 限制角色

            // 移除项
            if (!groupToSave.GroupAvailableRole.IsNullOrEmpty())
            {
                if (!groupInput.AvailableRoleIds.IsNullOrEmpty())
                {
                    List<GroupAvailableRole> roleToRemove = (from p in groupToSave.GroupAvailableRole
                                                             where !groupInput.AvailableRoleIds.Contains(p.RoleId)
                                                             select p).ToList();
                    for (int i = 0; i < roleToRemove.Count; i++)
                        groupToSave.GroupAvailableRole.Remove(roleToRemove[i]);
                }
                else
                    groupToSave.GroupAvailableRole.Clear();
            }
            // 添加项
            if (!groupInput.AvailableRoleIds.IsNullOrEmpty())
            {
                // 要添加的Id集
                List<Guid> roleIdToAdd = (from p in groupInput.AvailableRoleIds
                                          where groupToSave.GroupAvailableRole.All(m => m.RoleId != p)
                                          select p).ToList();

                // 要添加的项
                List<GroupAvailableRole> roleToAdd = await (from p in _context.Role
                                                            where roleIdToAdd.Contains(p.RoleId)
                                                            select new GroupAvailableRole
                                                            {

                                                                Role = p
                                                            }).ToListAsync();
                foreach (var item in roleToAdd)
                    groupToSave.GroupAvailableRole.Add(item);

            }

            #endregion

            #region 权限

            // 移除项
            if (!groupToSave.GroupPermission.IsNullOrEmpty())
            {
                if (!groupInput.PermissionIds.IsNullOrEmpty())
                {
                    List<GroupPermission> permissionToRemove = (from p in groupToSave.GroupPermission
                                                                where !groupInput.PermissionIds.Contains(p.PermissionId)
                                                                select p).ToList();
                    for (int i = 0; i < permissionToRemove.Count; i++)
                        groupToSave.GroupPermission.Remove(permissionToRemove[i]);
                }
                else
                {
                    groupToSave.GroupPermission.Clear();
                }
            }
            // 添加项
            if (!groupInput.PermissionIds.IsNullOrEmpty())
            {
                // 要添加的Id集
                List<Guid> permissionIdToAdd = (from p in groupInput.PermissionIds
                                                where groupToSave.GroupPermission.All(m => m.PermissionId != p)
                                                select p).ToList();

                // 要添加的项
                List<GroupPermission> permissionToAdd = await (from p in _context.Permission
                                                               where permissionIdToAdd.Contains(p.PermissionId)
                                                               select new GroupPermission
                                                               {
                                                                   Permission = p
                                                               }).ToListAsync();
                foreach (var item in permissionToAdd)
                    groupToSave.GroupPermission.Add(item);

            }

            #endregion

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// RemoveAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(Guid groupId, ModelStateDictionary modelState)
        {
            // 移除无限级分类步骤：

            // 1、获取预删节点信息
            Group groupToRemove = await _context.Group.FirstOrDefaultAsync(m => m.GroupId == groupId);
            // 当然，如果无法获取节点，属于无效操作；另外，超级管理员组和等待分配组不允许被删除
            if (groupToRemove == null)
            {
                modelState.AddModelError("GroupId", "尝试删除不存在的记录");
                return false;
            }
            if (groupToRemove.IsSystem)
            {
                modelState.AddModelError("GroupId", "当前节点是系统节点，不允许删除");
                return false;
            }

            // 2、节点包含子节点不允许删除
            if (await _context.Group.AnyAsync(m => m.ParentId == groupId))
            {
                modelState.AddModelError("GroupId", "当前节点存在子节点，不允许删除");
                return false;
            }

            // 3、更新用户表
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                // TODO: (alby)考虑允许用户不属于任何分组
                // 注：这里硬编码的节点是“待分配组”
                Guid targetGroupId = new Guid("11111111-1111-1111-1111-111111111111");

                var sql = "Update [User] Set GroupId=@TGroupId Where GroupId=@GroupId";
                await _context.Database.ExecuteSqlCommandAsync(sql,
                    new SqlParameter("GroupId", groupId)
                    , new SqlParameter("TGroupId", targetGroupId)
                    );

                // 1、更新DisplayOrder大于预删节点DisplayOrder的节点
                sql = "Update [Group] Set DisplayOrder=DisplayOrder-1 Where DisplayOrder>@DisplayOrder";
                await _context.Database.ExecuteSqlCommandAsync(sql,
                    new SqlParameter("DisplayOrder", groupToRemove.DisplayOrder)
                    );

                // 2、删除关联节点
                sql = "Delete [UserGroup] Where GroupId=@GroupId";
                await _context.Database.ExecuteSqlCommandAsync(sql,
                    new SqlParameter("GroupId", groupId)
                );

                // 3、删除关联节点
                sql = "Delete [GroupRole] Where GroupId=@GroupId";
                await _context.Database.ExecuteSqlCommandAsync(sql,
                    new SqlParameter("GroupId", groupId)
                );

                // 4、删除节点
                _context.Group.Remove(groupToRemove);
                await _context.SaveChangesAsync();

                dbContextTransaction.Commit();
            }

            return true;
        }

        /// <summary>
        /// MoveAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="movingTarget"></param>
        /// <returns></returns>
        public async Task<bool> MoveAsync(Guid groupId, MovingTarget movingTarget)
        {
            string sql;

            Group groupToMove = await _context.Group.FirstOrDefaultAsync(m => m.GroupId == groupId);
            // 保证DisplayOrder为 1 的“系统管理组”和“等待分配组”不被移动
            if (groupToMove == null || groupToMove.DisplayOrder <= 2) return false;

            #region 获取当前节点树(包含自身)

            List<Guid> currTreeIds;
            int displayOrderOfNextParentOrNextBrother = await GetDisplayOrderOfNextParentOrNextBrotherAsync(groupToMove);
            if (displayOrderOfNextParentOrNextBrother == 0)
            {
                currTreeIds = await _context.Group.Where(m => m.DisplayOrder >= groupToMove.DisplayOrder).Select(m => m.GroupId).ToListAsync();
            }
            else
            {
                currTreeIds = await _context.Group
                    .Where(m => m.DisplayOrder >= groupToMove.DisplayOrder && m.DisplayOrder < displayOrderOfNextParentOrNextBrother)
                    .Select(m => m.GroupId)
                    .ToListAsync();

            }
            int curTreeCount = currTreeIds.Count;

            #endregion

            if (MovingTarget.Up == movingTarget)
            {
                // 如果是处于两个系统节点之下的第一个节点，不允许上移
                if (groupToMove.DisplayOrder == 3) return false;

                #region 获取上一个兄弟节点

                Group targetGroup;
                if (groupToMove.ParentId.HasValue)
                    targetGroup = await _context.Group.OrderByDescending(m => m.DisplayOrder).FirstOrDefaultAsync(m =>
                    m.ParentId == groupToMove.ParentId && m.DisplayOrder < groupToMove.DisplayOrder);
                else
                    targetGroup = await _context.Group.OrderByDescending(m => m.DisplayOrder).FirstOrDefaultAsync(m =>
                    m.ParentId == null && m.DisplayOrder < groupToMove.DisplayOrder);

                #endregion

                if (targetGroup == null) return false;

                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    // 获取兄弟节点树的节点数
                    int targetTreeCount = await _context.Group.CountAsync(m =>
                    m.DisplayOrder >= targetGroup.DisplayOrder
                    && m.DisplayOrder < groupToMove.DisplayOrder);

                    // 更新兄弟节点树的DisplayOrder
                    sql = "Update [Group] Set DisplayOrder = DisplayOrder + @CurTreeCount Where DisplayOrder >= @TDisplayOrder And DisplayOrder<@CDisplayOrder";
                    await _context.Database.ExecuteSqlCommandAsync(sql
                        , new SqlParameter("CurTreeCount", curTreeCount)
                        , new SqlParameter("TDisplayOrder", targetGroup.DisplayOrder)
                        , new SqlParameter("CDisplayOrder", groupToMove.DisplayOrder)
                        );

                    sql = "Update [Group] Set DisplayOrder = DisplayOrder - @TargetTreeCount Where 1 <> 1 ";
                    foreach (var id in currTreeIds)
                        sql += $" Or GroupId = '{id}'";
                    await _context.Database.ExecuteSqlCommandAsync(sql
                        , new SqlParameter("TargetTreeCount", targetTreeCount)
                        );

                    dbContextTransaction.Commit();
                }
            }
            else
            {
                #region 获取下一个兄弟节点

                Group targetGroup;
                if (groupToMove.ParentId.HasValue)
                    targetGroup = await _context.Group.OrderBy(m => m.DisplayOrder).FirstOrDefaultAsync(m =>
                    m.ParentId == groupToMove.ParentId && m.DisplayOrder > groupToMove.DisplayOrder);
                else
                    targetGroup = await _context.Group.OrderBy(m => m.DisplayOrder).FirstOrDefaultAsync(m =>
                    m.ParentId == null && m.DisplayOrder > groupToMove.DisplayOrder);

                #endregion

                // 如果已经是最后一个节点，不允许下移
                if (targetGroup == null) return false;

                #region 获取兄弟节点树的节点数

                int displayOrderOfNextParentOrNextBrotherOfTarget = await GetDisplayOrderOfNextParentOrNextBrotherAsync(targetGroup);
                int targetTreeCount;
                if (displayOrderOfNextParentOrNextBrotherOfTarget == 0)
                    targetTreeCount = await _context.Group.CountAsync(m => m.DisplayOrder >= targetGroup.DisplayOrder);
                else
                    targetTreeCount = await _context.Group
                        .CountAsync(m => m.DisplayOrder >= targetGroup.DisplayOrder && m.DisplayOrder < displayOrderOfNextParentOrNextBrotherOfTarget);

                #endregion

                using (var dbContextTransaction = _context.Database.BeginTransaction())
                {
                    // 更新兄弟节点树的DisplayOrder
                    sql = "Update [Group] Set DisplayOrder = DisplayOrder - @CurTreeCount Where DisplayOrder >= @DisplayOrder And DisplayOrder < @TDisplayOrder";

                    await _context.Database.ExecuteSqlCommandAsync(sql
                        , new SqlParameter("CurTreeCount", curTreeCount)
                        , new SqlParameter("DisplayOrder", targetGroup.DisplayOrder)
                        , new SqlParameter("TDisplayOrder", targetGroup.DisplayOrder + targetTreeCount)
                        );

                    sql = "Update [Group] Set DisplayOrder = DisplayOrder + @TargetTreeCount Where 1 <> 1 ";
                    foreach (var id in currTreeIds)
                        sql += $" Or GroupId = '{id}'";

                    await _context.Database.ExecuteSqlCommandAsync(sql
                        , new SqlParameter("TargetTreeCount", targetTreeCount)
                        );

                    dbContextTransaction.Commit();
                }

            }
            return true;
        }

        /// <summary>
        /// MoveAsync
        /// </summary>
        /// <param name="sourceGroupId"></param>
        /// <param name="targetGroupId"></param>
        /// <param name="movingLocation"></param>
        /// <param name="isChild"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> MoveAsync(Guid sourceGroupId, Guid targetGroupId, MovingLocation movingLocation, bool? isChild, ModelStateDictionary modelState)
        {
            if (sourceGroupId == targetGroupId)
            {
                modelState.AddModelError("SourceGroupId", "源节点Id和目标节点Id不能相同");
                return false;
            }
            var sourceGroup = await _context.Group.FirstOrDefaultAsync(m => m.GroupId == sourceGroupId);
            if (sourceGroup == null)
            {
                modelState.AddModelError("SourceGroupId", "源节点不存在");
                return false;
            }
            var targetGroup = await _context.Group.FirstOrDefaultAsync(m => m.GroupId == targetGroupId);
            if (targetGroup == null)
            {
                modelState.AddModelError("TargetGroupId", "目标节点不存在");
                return false;
            }

            return await MoveAsync(sourceGroup, targetGroup, movingLocation, isChild, modelState);
        }

        /// <summary>
        /// MoveByDisplayOrderAsync
        /// </summary>
        /// <param name="sourceDisplayOrder"></param>
        /// <param name="targetDisplayOrder"></param>
        /// <param name="movingLocation"></param>
        /// <param name="isChild"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> MoveByDisplayOrderAsync(int sourceDisplayOrder, int targetDisplayOrder, MovingLocation movingLocation, bool? isChild, ModelStateDictionary modelState)
        {
            if (sourceDisplayOrder == targetDisplayOrder)
            {
                modelState.AddModelError("SourceDisplayOrder", "源节点的DisplayOrder和目标节点的DisplayOrder不能相同");
                return false;
            }
            var sourceGroup = await _context.Group.FirstOrDefaultAsync(m => m.DisplayOrder == sourceDisplayOrder);
            if (sourceGroup == null)
            {
                modelState.AddModelError("SourceDisplayOrder", "源节点不存在");
                return false;
            }
            var targetGroup = await _context.Group.FirstOrDefaultAsync(m => m.DisplayOrder == targetDisplayOrder);
            if (targetGroup == null)
            {
                modelState.AddModelError("TargetDisplayOrder", "目标节点不存在");
                return false;
            }
            return await MoveAsync(sourceGroup, targetGroup, movingLocation, isChild, modelState);
        }

        /// <summary>
        /// MoveAsync
        /// </summary>
        /// <param name="sourceGroup"></param>
        /// <param name="targetGroup"></param>
        /// <param name="movingLocation"></param>
        /// <param name="isChild"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        private async Task<bool> MoveAsync(Group sourceGroup, Group targetGroup, MovingLocation movingLocation, bool? isChild, ModelStateDictionary modelState)
        {
            #region 数据验证: 基本

            if (sourceGroup.DisplayOrder == targetGroup.DisplayOrder)
            {
                modelState.AddModelError("SourceGroupId", "源DisplayOrder和目标DisplayOrder不能相同");
                return false;
            }
            // 不允许移动两个系统节点
            if (sourceGroup.DisplayOrder <= 2)
            {
                modelState.AddModelError("SourceGroupId", "不允许移动两个系统节点");
                return false;
            }
            // 不允许移动到两个系统节点之前
            if (movingLocation == MovingLocation.Above && targetGroup.DisplayOrder <= 2)
            {
                modelState.AddModelError("SourceGroupId", "不允许移动到两个系统节点之前");
                return false;
            }
            // 不允许移动到两个系统节点之间
            if (movingLocation == MovingLocation.Under && targetGroup.DisplayOrder == 1)
            {
                modelState.AddModelError("SourceGroupId", "不允许移动到两个系统节点之间");
                return false;
            }
            // 不允许移动到节点前面而作为子节点
            if (movingLocation == MovingLocation.Above && isChild.HasValue && isChild.Value)
            {
                modelState.AddModelError("SourceGroupId", "不允许移动到节点前面而作为子节点");
                return false;
            }

            #endregion

            // 获取当前节点的下一个兄弟节点或更高层下一个父节点 (不是自己的父节点)的DisplayOrder
            int displayOrderOfNextParentOrNextBrother = await GetDisplayOrderOfNextParentOrNextBrotherAsync(sourceGroup);
            // 当前节点树Id集合 (包含本节点)
            var sourceTree = await GetTreeAsync(sourceGroup, displayOrderOfNextParentOrNextBrother, true);

            #region 数据验证: 贪吃蛇

            if (sourceTree.Any(m => m.GroupId == targetGroup.GroupId))
            {
                modelState.AddModelError("SourceGroupId", "源节点不能是目标节点的直接或间接父节点");
                return false;
            }

            #endregion

            #region 数据验证: 如果节点已经在指定的位置，则直接报错返回。

            if (movingLocation == MovingLocation.Above /*&& (!isChild.HasValue || !isChild.Value)*/ && sourceGroup.ParentId == targetGroup.ParentId && displayOrderOfNextParentOrNextBrother == targetGroup.DisplayOrder)
            {
                modelState.AddModelError("SourceGroupId", "源节点已经作为目标节点的上一个兄弟节点存在");
                return false;
            }
            if (movingLocation == MovingLocation.Under && (isChild.HasValue && isChild.Value) && sourceGroup.ParentId == targetGroup.GroupId && sourceGroup.DisplayOrder == targetGroup.DisplayOrder + 1)
            {
                modelState.AddModelError("SourceGroupId", "源节点已经作为目标节点的第一个节点存在");
                return false;
            }
            if (movingLocation == MovingLocation.Under && (!isChild.HasValue || !isChild.Value) && sourceGroup.ParentId == targetGroup.ParentId && sourceGroup.DisplayOrder == await GetDisplayOrderOfNextParentOrNextBrotherAsync(targetGroup))
            {
                modelState.AddModelError("SourceGroupId", "源节点已经作为目标节点的下一个兄弟节点存在");
                return false;
            }

            #endregion

            // 备注：
            // 1、节点移动到另一个节点之上，实际上就是作为兄弟节点。
            // 2、节点移动到另一个节点之下，如果是作为子节点，实际上就是目前编辑节点，选择另一个节点作为父节点的操作。
            // 3、isChild 影响 ParentId 和 Level。

            var maxDisplayOrderInSourceTree = sourceGroup.DisplayOrder + sourceTree.Count - 1;

            // 用于移动其他节点
            int moveTargetDisplayOrderMin;
            int moveTargetDisplayOrderMax;
            int moveTargetXDisplayOrder;

            int xDisplayOrder;
            int xLevel;
            Guid? sourceNewParentId = isChild.HasValue && isChild.Value ? targetGroup.GroupId : targetGroup.ParentId;

            if (sourceGroup.ParentId == targetGroup.GroupId && movingLocation == MovingLocation.Under && (!isChild.HasValue || !isChild.Value))
            {
                // 平移算法：
                // 如果 sourceGroup 之前是 targetGroup 的子节点，转换为非子节点。targetGroup 的相关的子节点向上移动。sourceGroup 节点树向下移动。
                // 1、本节点树以下、目标在“本节点树”之后的节点的上移 DisplayOrder = DisplayOrder - 本节点树的节点数
                // 2、本节点树的 DisplayOrder = DisplayOrder + (1 中移动的节点数)
                // 3、本节点树的 Level = Level + xLevel
                // 4、本节点的 ParentId = sourceNewParentId

                moveTargetDisplayOrderMin = maxDisplayOrderInSourceTree + 1;
                moveTargetDisplayOrderMax = await GetMaxDisplayOrderInTreeAsync(targetGroup);
                moveTargetXDisplayOrder = -sourceTree.Count;

                var moveTargetNodeCount = moveTargetDisplayOrderMax - moveTargetDisplayOrderMin + 1;
                xDisplayOrder = moveTargetNodeCount;

                xLevel = targetGroup.Level - sourceGroup.Level; // xLevel 永远为 -1
            }
            else if (targetGroup.DisplayOrder - sourceGroup.DisplayOrder > 0)
            {
                // 下移算法： 
                // 1、本节点树以下、目标节点 (包括或不包括)之上的节点的上移 DisplayOrder = DisplayOrder - 本节点树的节点数
                // 2、本节点树的 DisplayOrder = DisplayOrder + (1 中移动的节点数)
                // 3、本节点树的 Level = Level + xLevel
                // 4、本节点的 ParentId = sourceNewParentId

                moveTargetDisplayOrderMin = maxDisplayOrderInSourceTree + 1;
                moveTargetDisplayOrderMax = targetGroup.DisplayOrder - (movingLocation == MovingLocation.Above ? 1 : 0); // 放入目标节点下面，则包括目标节点本身
                moveTargetXDisplayOrder = -sourceTree.Count;

                var moveTargetNodeCount = moveTargetDisplayOrderMax - moveTargetDisplayOrderMin + 1;
                xDisplayOrder = moveTargetNodeCount;

                xLevel = targetGroup.Level - sourceGroup.Level + (isChild.HasValue && isChild.Value ? 1 : 0);
            }
            else
            {
                // 上移算法：
                // 1、本节点树以上、目标节点之下 (包括或不包括)的节点的下移 DisplayOrder = DisplayOrder + 本节点树的节点数
                // 2、本节点树的 DisplayOrder = DisplayOrder + (1 中移动的节点数)
                // 3、本节点树的 Level = Level + xLevel
                // 4、本节点的 ParentId = sourceNewParentId

                moveTargetDisplayOrderMin = targetGroup.DisplayOrder + (movingLocation == MovingLocation.Above ? 0 : 1); // 放入目标节点上面，则包括目标节点本身
                moveTargetDisplayOrderMax = sourceGroup.DisplayOrder - 1;
                moveTargetXDisplayOrder = sourceTree.Count;

                var moveTargetNodeCount = moveTargetDisplayOrderMax - moveTargetDisplayOrderMin + 1;
                xDisplayOrder = -moveTargetNodeCount;

                xLevel = targetGroup.Level - sourceGroup.Level + (isChild.HasValue && isChild.Value ? 1 : 0);
            }

            #region 保存

            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                var sql = "Update [Group] Set DisplayOrder = DisplayOrder + @MoveTargetXDisplayOrder Where DisplayOrder >= @MoveTargetDisplayOrderMin And DisplayOrder <= @MoveTargetDisplayOrderMax";
                await _context.Database.ExecuteSqlCommandAsync(sql
                    , new SqlParameter("MoveTargetXDisplayOrder", moveTargetXDisplayOrder)
                    , new SqlParameter("MoveTargetDisplayOrderMin", moveTargetDisplayOrderMin)
                    , new SqlParameter("MoveTargetDisplayOrderMax", moveTargetDisplayOrderMax)
                    );

                sourceTree.ForEach(m =>
                {
                    m.DisplayOrder += xDisplayOrder;
                    m.Level += xLevel;
                });

                sourceGroup.ParentId = sourceNewParentId;
                await _context.SaveChangesAsync();

                dbContextTransaction.Commit();
            }

            #endregion

            return true;
        }

        #endregion

        #region Private Methods

        private async Task<List<Guid>> GetTreeIdListAsync(Guid groupId, bool isIncludeSelf)
        {
            var group = await _context.Group.FirstOrDefaultAsync(m => m.GroupId == groupId);
            if (group == null)
                return new List<Guid>(0);

            return await GetTreeIdListAsync(group, isIncludeSelf);
        }

        private async Task<List<Guid>> GetTreeIdListAsync(Group group, bool isIncludeSelf)
        {
            int displayOrderOfNextParentOrNextBrother = await GetDisplayOrderOfNextParentOrNextBrotherAsync(group);

            return await GetTreeIdListAsync(group, displayOrderOfNextParentOrNextBrother, isIncludeSelf);
        }

        private async Task<List<Guid>> GetTreeIdListAsync(Group group, int displayOrderOfNextParentOrNextBrother, bool isIncludeSelf)
        {
            List<Guid> list;
            if (displayOrderOfNextParentOrNextBrother == 0)
            {
                //说明当前节点是最后一个节点,直接获取
                list = await
                    _context.Group.Where(m => m.DisplayOrder > group.DisplayOrder)
                        .Select(m => m.GroupId)
                        .ToListAsync();
            }
            else
            {
                list = await
                    _context.Group.Where(m => m.DisplayOrder > group.DisplayOrder && m.DisplayOrder < displayOrderOfNextParentOrNextBrother)
                        .Select(m => m.GroupId)
                        .ToListAsync();
            }

            if (isIncludeSelf)
            {
                list.Insert(0, group.GroupId);
            }

            return list;
        }

        private async Task<List<Group>> GetTreeAsync(Guid groupId, bool isIncludeSelf)
        {
            var group = await _context.Group.FirstOrDefaultAsync(m => m.GroupId == groupId);
            if (group == null)
                return new List<Group>(0);

            return await GetTreeAsync(group, isIncludeSelf);
        }

        private async Task<List<Group>> GetTreeAsync(Group group, bool isIncludeSelf)
        {
            int displayOrderOfNextParentOrNextBrother = await GetDisplayOrderOfNextParentOrNextBrotherAsync(group);

            return await GetTreeAsync(group, displayOrderOfNextParentOrNextBrother, isIncludeSelf);
        }

        private async Task<List<Group>> GetTreeAsync(Group group, int displayOrderOfNextParentOrNextBrother, bool isIncludeSelf)
        {
            List<Group> list;
            if (displayOrderOfNextParentOrNextBrother != 0)
                list = await _context.Group.Where(m => m.DisplayOrder > group.DisplayOrder && m.DisplayOrder < displayOrderOfNextParentOrNextBrother)
                    .OrderBy(m => m.DisplayOrder)
                    .ToListAsync();
            else
                list = await _context.Group.Where(m => m.DisplayOrder > group.DisplayOrder)
                    .OrderBy(m => m.DisplayOrder)
                    .ToListAsync();

            if (isIncludeSelf)
            {
                list.Insert(0, group);
            }

            return list;
        }

        private async Task<int> GetTreeNodeCountAsync(Guid groupId, bool isIncludeSelf)
        {
            var group = await _context.Group.FirstOrDefaultAsync(m => m.GroupId == groupId);
            if (group == null)
                return 0;

            return await GetTreeNodeCountAsync(group, isIncludeSelf);
        }

        private async Task<int> GetTreeNodeCountAsync(Group group, bool isIncludeSelf)
        {
            int displayOrderOfNextParentOrNextBrother = await GetDisplayOrderOfNextParentOrNextBrotherAsync(group);

            int count;
            if (displayOrderOfNextParentOrNextBrother == 0)
            {
                //说明当前节点是最后一个节点,直接获取
                count = 0;
            }
            else
            {
                count = await _context.Group.Where(m => m.DisplayOrder > group.DisplayOrder && m.DisplayOrder < displayOrderOfNextParentOrNextBrother).
                    OrderByDescending(m => m.DisplayOrder).Select(m => m.DisplayOrder).FirstOrDefaultAsync();
            }

            if (isIncludeSelf)
            {
                count++;
            }

            return count;
        }

        private async Task<int> GetDisplayOrderOfNextParentOrNextBrotherAsync(Guid groupId)
        {
            var group = await _context.Group.FirstOrDefaultAsync(m => m.GroupId == groupId);
            if (group == null)
                return 0;

            return await GetDisplayOrderOfNextParentOrNextBrotherAsync(group);
        }

        private async Task<int> GetDisplayOrderOfNextParentOrNextBrotherAsync(Group group)
        {
            return await GetDisplayOrderOfNextParentOrNextBrotherAsync(group.DisplayOrder, group.Level);
        }

        private async Task<int> GetDisplayOrderOfNextParentOrNextBrotherAsync(int displayOrder, int level)
        {
            return await _context.Group.Where(m => m.Level <= level && m.DisplayOrder > displayOrder)
                .OrderBy(m => m.DisplayOrder)
                .Select(m => m.DisplayOrder)
                .FirstOrDefaultAsync();
        }

        private async Task<int> GetMaxDisplayOrderAsync()
        {
            return await _context.Group.MaxAsync(m => (int?)m.DisplayOrder) ?? 0;
        }

        private async Task<int> GetMaxDisplayOrderInTreeAsync(Guid groupId)
        {
            var group = await _context.Group.FirstOrDefaultAsync(m => m.GroupId == groupId);
            if (group == null)
                throw new NullReferenceException("节点不存在");

            return await GetMaxDisplayOrderInTreeAsync(group);
        }

        private async Task<int> GetMaxDisplayOrderInTreeAsync(Group group)
        {
            int maxDisplayOrder;

            // 获取父节点之下的兄弟节点或更高层次的父节点(不是自己的父节点)的DisplayOrder
            int displayOrderOfNextParentOrNextBrother = await GetDisplayOrderOfNextParentOrNextBrotherAsync(group);

            if (displayOrderOfNextParentOrNextBrother == 0)
                maxDisplayOrder = await _context.Group.Where(m => m.DisplayOrder > group.DisplayOrder || m.GroupId == group.GroupId).MaxAsync(m => m.DisplayOrder);
            else
                maxDisplayOrder = displayOrderOfNextParentOrNextBrother - 1;

            return maxDisplayOrder;
        }

        #endregion
    }
}
