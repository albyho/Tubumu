using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Api;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;
using Group = Tubumu.Modules.Admin.Models.Group;
using Permission = Tubumu.Modules.Admin.Models.Permission;

namespace Tubumu.Modules.Admin.Controllers
{
    public partial class AdminController
    {
        #region 用户管理

        #region 用户

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost("GetUsers")]
        [PermissionAuthorize(Permissions = "用户管理")]
        public async Task<ApiPageResult<UserInfo>> GetUsers([FromBody]UserSearchCriteria criteria)
        {
            var result = new ApiPageResult<UserInfo>();
            var page = await _userService.GetPageAsync(criteria);
            result.Code = 200;
            result.Message = "获取用户列表成功";
            result.Page = page;

            return result;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        [HttpPost("AddUser")]
        [PermissionAuthorize(Permissions = "用户管理")]
        public async Task<ApiResult> AddUser([FromBody]UserInputAdd userInput)
        {
            var result = new ApiResult();

            if (await _userService.SaveAsync(userInput, ModelState) == null)
            {
                result.Code = 400;
                result.Message = "添加用户失败：" + ModelState.FirstErrorMessage();
                return result;
            }

            result.Code = 200;
            result.Message = "添加用户成功";
            return result;
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        [HttpPost("EditUser")]
        [PermissionAuthorize(Permissions = "用户管理")]
        public async Task<ApiResult> EditUser([FromBody]UserInputEdit userInput)
        {
            var result = new ApiResult();
            if (!ModelState.IsValid)
            {
                result.Code = 400;
                result.Message = "编辑用户失败：" + ModelState.FirstErrorMessage();
                return result;
            }

            if (!userInput.UserId.HasValue)
            {
                result.Code = 400;
                result.Message = "编辑用户失败：必须提供参数 GroupId";
                return result;
            }

            if (await _userService.SaveAsync(userInput, ModelState) == null)
            {
                result.Code = 400;
                result.Message = "编辑用户失败：" + ModelState.FirstErrorMessage();
                return result;
            }

            result.Code = 200;
            result.Message = "编辑用户成功";
            return result;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userIdInput"></param>
        /// <returns></returns>
        [HttpPost("RemoveUser")]
        [PermissionAuthorize(Permissions = "用户管理")]
        public async Task<ApiResult> RemoveUser([FromBody]UserIdInput userIdInput)
        {
            var result = new ApiResult();
            if (!await _userService.RemoveAsync(userIdInput.UserId, ModelState))
            {
                result.Code = 400;
                result.Message = "删除失败：" + ModelState.FirstErrorMessage();
                return result;
            }

            result.Code = 200;
            result.Message = "删除成功";
            return result;
        }

        #endregion

        #region 分组

        /// <summary>
        /// 获取分组列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetGroups")]
        [PermissionAuthorize(Permissions = "分组管理")]
        public async Task<ApiListResult<List<Group>>> GetGroups()
        {
            var groups = await _groupService.GetListInCacheAsync();
            ProjectGroups(groups);
            var result = new ApiListResult<List<Group>>
            {
                Code = 200,
                Message = "获取分组列表成功",
                List = groups,
            };

            return result;
        }

        /// <summary>
        /// 获取分组树
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetGroupTree")]
        [PermissionAuthorize(Permissions = "分组管理")]
        public async Task<GroupTreeResult> GetGroupTree()
        {
            var groups = await _groupService.GetListInCacheAsync();
            var tree = new List<GroupTreeNode>();
            for (var i = 0; i < groups.Count; i++)
            {
                var item = groups[i];
                if (item.Level == 1)
                {
                    var node = GroupTreeNodeFromGroup(item);
                    node.ParentIdPath = null;
                    tree.Add(node);
                    GroupTreeAddChildren(groups, node, i);
                }
            }
            var result = new GroupTreeResult
            {
                Code = 200,
                Message = "获取分组树成功",
                Tree = tree,
            };

            return result;
        }

        /// <summary>
        /// 添加分组
        /// </summary>
        /// <param name="groupInput"></param>
        /// <returns></returns>
        [HttpPost("AddGroup")]
        [PermissionAuthorize(Permissions = "分组管理")]
        public async Task<ApiResult> AddGroup([FromBody]GroupInput groupInput)
        {
            var result = new ApiResult();
            if (groupInput.GroupId.HasValue)
            {
                // Guid.Empty 也不允许
                result.Code = 400;
                result.Message = "添加分组失败：无需提供参数 GroupId";
                return result;
            }
            if (!await _groupService.SaveAsync(groupInput, ModelState))
            {
                result.Code = 400;
                result.Message = "添加分组失败：" + ModelState.FirstErrorMessage();
                return result;
            }

            result.Code = 200;
            result.Message = "添加分组成功";
            return result;
        }

        /// <summary>
        /// 编辑分组
        /// </summary>
        /// <param name="groupInput"></param>
        /// <returns></returns>
        [HttpPost("EditGroup")]
        [PermissionAuthorize(Permissions = "分组管理")]
        public async Task<ApiResult> EditGroup([FromBody]GroupInput groupInput)
        {
            var result = new ApiResult();
            if (groupInput.GroupId.IsNullOrEmpty())
            {
                result.Code = 400;
                result.Message = "编辑分组失败：必须提供参数 GroupId";
                return result;
            }

            if (!await _groupService.SaveAsync(groupInput, ModelState))
            {
                result.Code = 400;
                result.Message = "编辑分组失败：" + ModelState.FirstErrorMessage();
                return result;
            }

            result.Code = 200;
            result.Message = "编辑分组成功";
            return result;
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="groupIdInput"></param>
        /// <returns></returns>
        [HttpPost("RemoveGroup")]
        [PermissionAuthorize(Permissions = "分组管理")]
        public async Task<ApiResult> RemoveGroup([FromBody]GroupIdInput groupIdInput)
        {
            var result = new ApiResult();

            if (await _groupService.RemoveAsync(groupIdInput.GroupId, ModelState))
            {
                result.Code = 200;
                result.Message = "删除成功";
            }
            else
            {
                result.Code = 400;
                result.Message = "删除失败：" + ModelState.FirstErrorMessage();
            }

            return result;
        }

        /// <summary>
        /// 移动分组(排序)
        /// </summary>
        /// <param name="moveGroupInput"></param>
        /// <returns></returns>
        [HttpPost("MoveGroup")]
        [PermissionAuthorize(Permissions = "分组管理")]
        public async Task<ApiResult> MoveGroup(MoveGroupInput moveGroupInput)
        {
            var result = new ApiResult();

            if (await _groupService.MoveAsync(moveGroupInput.SourceId, moveGroupInput.TargetId, moveGroupInput.MovingLocation, moveGroupInput.IsChild, ModelState))
            {
                result.Code = 200;
                result.Message = "移动成功";
            }
            else
            {
                result.Code = 400;
                result.Message = "移动失败：" + ModelState.FirstErrorMessage();
            }

            return result;
        }

        #endregion

        #region 角色管理

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRoles")]
        public async Task<ApiListResult<List<Role>>> GetRoles()
        {
            var roles = await _roleService.GetListInCacheAsync();
            var result = new ApiListResult<List<Role>>
            {
                Code = 200,
                Message = "获取角色列表成功",
                List = roles,
            };

            return result;
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRoleBases")]
        public async Task<ApiListResult<List<RoleBase>>> GetRoleBases()
        {
            var roles = await _roleService.GetBaseListInCacheAsync();
            var result = new ApiListResult<List<RoleBase>>
            {
                Code = 200,
                Message = "获取角色列表成功",
                List = roles,
            };

            return result;
        }

        /// <summary>
        /// 保存角色名称
        /// </summary>
        /// <param name="saveRoleNameInput"></param>
        /// <returns></returns>
        [HttpPost("SaveRoleName")]
        [PermissionAuthorize(Permissions = "角色管理")]
        public async Task<ApiResult> SaveRoleName([FromBody]RoleNameInput saveRoleNameInput)
        {
            var result = new ApiResult();
            if (!await _roleService.EditNameAsync(saveRoleNameInput, ModelState))
            {
                result.Code = 400;
                result.Message = "编辑名称失败：" + ModelState.FirstErrorMessage();
            }
            else
            {
                result.Code = 200;
                result.Message = "编辑名称成功";
            }

            return result;
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="roleInput"></param>
        /// <returns></returns>
        [HttpPost("EditRole")]
        [PermissionAuthorize(Permissions = "角色管理")]
        public async Task<ApiItemResult<Role>> EditRole([FromBody]RoleInput roleInput)
        {
            var result = new ApiItemResult<Role>();
            if (!ModelState.IsValid)
            {
                result.Code = 400;
                result.Message = "编辑角色失败：" + ModelState.FirstErrorMessage();
                return result;
            }

            if (roleInput.RoleId.IsNullOrEmpty())
            {
                result.Code = 400;
                result.Message = "编辑角色失败：必须提供参数 RoleId";
                return result;
            }

            var role = await _roleService.SaveAsync(roleInput, ModelState);
            if (role == null)
            {
                result.Code = 400;
                result.Message = "编辑角色失败：" + ModelState.FirstErrorMessage();
                return result;
            }

            result.Code = 200;
            result.Message = "编辑角色成功";
            result.Item = role;
            return result;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="roleInput"></param>
        /// <returns></returns>
        [HttpPost("AddRole")]
        [PermissionAuthorize(Permissions = "角色管理")]
        public async Task<ApiItemResult<Role>> AddRole([FromBody]RoleInput roleInput)
        {
            var result = new ApiItemResult<Role>();
            if (roleInput.RoleId.HasValue)
            {
                // Guid.Empty 也不允许
                result.Code = 400;
                result.Message = "添加角色失败：无需提供参数 RoleId";
                return result;
            }
            var role = await _roleService.SaveAsync(roleInput, ModelState);
            if (role == null)
            {
                result.Code = 400;
                result.Message = "添加角色失败：" + ModelState.FirstErrorMessage();
                return result;
            }

            result.Code = 200;
            result.Message = "添加角色成功";
            result.Item = role;
            return result;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleIdInput"></param>
        /// <returns></returns>
        [HttpPost("RemoveRole")]
        [PermissionAuthorize(Permissions = "角色管理")]
        public async Task<ApiResult> RemoveRole([FromBody]RoleIdInput roleIdInput)
        {
            var result = new ApiResult();

            if (await _roleService.RemoveAsync(roleIdInput.RoleId, ModelState))
            {
                result.Code = 200;
                result.Message = "删除成功";
            }
            else
            {
                result.Code = 400;
                result.Message = "删除失败：" + ModelState.FirstErrorMessage();
            }

            return result;
        }

        /// <summary>
        /// 移动角色(排序)
        /// </summary>
        /// <param name="moveRoleInput"></param>
        /// <returns></returns>
        [HttpPost("MoveRole")]
        [PermissionAuthorize(Permissions = "角色管理")]
        public async Task<ApiResult> MoveRole([FromBody]MoveRoleInput moveRoleInput)
        {
            var result = new ApiResult();

            if (await _roleService.MoveAsync(moveRoleInput.SourceDisplayOrder, moveRoleInput.TargetDisplayOrder, ModelState))
            {
                result.Code = 200;
                result.Message = "排序成功";
            }
            else
            {
                result.Code = 400;
                result.Message = "排序失败：" + ModelState.FirstErrorMessage();
            }

            return result;
        }

        #endregion

        #region 权限管理

        /// <summary>
        /// 获取权限树
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPermissionTree")]
        public async Task<ApiTreeResult> GetPermissionTree()
        {
            var permissions = await _permissionService.GetListInCacheAsync();
            var tree = new List<TreeNode>();
            for (var i = 0; i < permissions.Count; i++)
            {
                var item = permissions[i];
                if (item.Level == 1)
                {
                    var node = new TreeNode
                    {
                        Id = item.PermissionId,
                        Name = item.Name
                    };
                    tree.Add(node);
                    PermissionTreeAddChildren(permissions, node, i);
                }
            }
            var result = new ApiTreeResult
            {
                Code = 200,
                Message = "获取权限树成功",
                Tree = tree,
            };

            return result;
        }

        #endregion

        #region 用户状态

        /// <summary>
        /// 获取用户状态
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserStatus")]
        public ApiListResult<IEnumerable<KeyValuePair<UserStatus, string>>> GetUserStatus()
        {
            var list = typeof(UserStatus).GetEnumDictionary<UserStatus>();
            var result = new ApiListResult<IEnumerable<KeyValuePair<UserStatus, string>>>
            {
                Code = 200,
                Message = "获取用户状态列表成功",
                List = list,
            };

            return result;
        }

        #endregion

        #endregion

        #region Private Methods

        private void ProjectPermissions(List<Permission> permissions)
        {
            if (permissions == null)
            {
                permissions = new List<Permission>();
                return;
            }
            string s = "　";

            foreach (var p in permissions)
            {
                if (p.Level > 1)
                    p.Name = s.Repeat(p.Level - 1) + "┗ " + p.Name;
            }
        }

        private void PermissionTreeAddChildren(List<Permission> permissions, TreeNode node, int index)
        {
            for (var i = index + 1; i < permissions.Count; i++)
            {
                var item = permissions[i];
                if (item.ParentId == node.Id)
                {
                    if (node.Children == null)
                    {
                        node.Children = new List<TreeNode>();
                    };
                    var child = new TreeNode
                    {
                        Id = item.PermissionId,
                        Name = item.Name
                    };
                    node.Children.Add(child);
                    PermissionTreeAddChildren(permissions, child, i);
                }
            }
        }

        private void ProjectGroups(List<Group> groups)
        {
            if (groups == null)
            {
                groups = new List<Group>();
                return;
            }
            string s = "　";

            foreach (var p in groups)
            {
                if (p.Level > 1)
                    p.Name = s.Repeat(p.Level - 1) + "┗ " + p.Name;
            }
        }

        private void GroupTreeAddChildren(List<Group> groups, GroupTreeNode node, int index)
        {
            for (var i = index + 1; i < groups.Count; i++)
            {
                var item = groups[i];
                if (item.ParentId == node.Id)
                {
                    if (node.Children == null)
                    {
                        node.Children = new List<GroupTreeNode>();
                    };
                    var child = GroupTreeNodeFromGroup(item);
                    // 在父节点的 ParentIdPath 基础上增加 ParentId
                    child.ParentIdPath = node.ParentIdPath != null ? new List<Guid>(node.ParentIdPath) : new List<Guid>(1);
                    child.ParentIdPath.Add(node.Id);
                    node.Children.Add(child);
                    GroupTreeAddChildren(groups, child, i);
                }
            }
        }

        private GroupTreeNode GroupTreeNodeFromGroup(Group group)
        {
            return new GroupTreeNode
            {
                Id = group.GroupId,
                ParentId = group.ParentId,
                Name = group.Name,
                Level = group.Level,
                DisplayOrder = group.DisplayOrder,
                IsSystem = group.IsSystem,
                IsContainsUser = group.IsContainsUser,
                IsDisabled = group.IsDisabled,
                Roles = group.Roles,
                AvailableRoles = group.AvailableRoles,
                Permissions = group.Permissions,
            };
        }

        #endregion
    }
}
