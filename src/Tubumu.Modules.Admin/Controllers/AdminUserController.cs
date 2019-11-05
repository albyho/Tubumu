using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Api;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Core.Models;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;
using Group = Tubumu.Modules.Admin.Models.Group;
using Permission = Tubumu.Modules.Admin.Models.Permission;
using Role = Tubumu.Modules.Admin.Models.Role;

namespace Tubumu.Modules.Admin.Controllers
{
    /// <summary>
    /// 后台：用户管理
    /// </summary>
    public partial class AdminController
    {
        #region 用户管理

        #region 用户

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [HttpPost("GetUserPage")]
        [TubumuAuthorize(Permissions = "用户管理")]
        public async Task<ApiResultData<Page<UserInfo>>> GetUserPage(UserPageSearchCriteria criteria)
        {
            var result = new ApiResultData<Page<UserInfo>>();
            var page = await _userService.GetPageAsync(criteria);
            result.Code = 200;
            result.Message = "获取用户列表成功";
            result.Data = page;

            return result;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        [HttpPost("AddUser")]
        [TubumuAuthorize(Permissions = "用户管理")]
        public async Task<ApiResult> AddUser(UserInputAdd userInput)
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
        [TubumuAuthorize(Permissions = "用户管理")]
        public async Task<ApiResult> EditUser(UserInputEdit userInput)
        {
            var result = new ApiResult();
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
        [TubumuAuthorize(Permissions = "用户管理")]
        public async Task<ApiResult> RemoveUser(UserIdInput userIdInput)
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

        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <param name="userImageInput"></param>
        /// <returns></returns>
        [HttpPost("ChangeUserAvatar")]
        [TubumuAuthorize(Permissions = "用户管理")]
        public async Task<ApiResultUrl> ChangeUserAvatar([FromForm]UserImageInput userImageInput)
        {
            var result = new ApiResultUrl();
            var url = await _userService.ChangeAvatarAsync(userImageInput, ModelState);
            if (!ModelState.IsValid)
            {
                result.Code = 400;
                result.Message = $"修改头像失败:{ModelState.FirstErrorMessage()}";
                return result;
            }

            result.Url = url;
            result.Code = 200;
            result.Message = "修改头像成功";
            return result;
        }

        /// <summary>
        /// 修改用户 Logo
        /// </summary>
        /// <param name="userImageInput"></param>
        /// <returns></returns>
        [HttpPost("ChangeUserLogo")]
        [TubumuAuthorize(Permissions = "用户管理")]
        public async Task<ApiResultUrl> ChangeUserLogo([FromForm]UserImageInput userImageInput)
        {
            var result = new ApiResultUrl();
            var url = await _userService.ChangeLogoAsync(userImageInput, ModelState);
            if (!ModelState.IsValid)
            {
                result.Code = 400;
                result.Message = $"修改 Logo 失败:{ModelState.FirstErrorMessage()}";
                return result;
            }

            result.Url = url;
            result.Code = 200;
            result.Message = "修改 Logo 成功";
            return result;
        }

        /// <summary>
        /// 验证用户名是否被使用
        /// </summary>
        /// <param name="validateUsernameExistsInput"></param>
        /// <returns></returns>
        [HttpPost("ValidateUsernameExists")]
        [TubumuAuthorize(Permissions = "用户管理")]
        public async Task<ApiResult> ValidateUsernameExists(ValidateUsernameExistsInput validateUsernameExistsInput)
        {
            var result = new ApiResult();
            var isExists = await _userService.IsExistsUsernameAsync(validateUsernameExistsInput.Username, validateUsernameExistsInput.UserId);
            if (!isExists)
            {
                result.Code = 200;
                result.Message = "验证通过";
            }
            else
            {
                result.Code = 400;
                result.Message = "用户名已经被使用";
            }
            return result;
        }

        /// <summary>
        /// 验证手机号是否被使用
        /// </summary>
        /// <param name="validateMobileExistsInput"></param>
        /// <returns></returns>
        [HttpPost("ValidateMobileExists")]
        [TubumuAuthorize(Permissions = "用户管理")]
        public async Task<ApiResult> ValidateMobileExists(ValidateMobileExistsInput validateMobileExistsInput)
        {
            var result = new ApiResult();
            var isExists = await _userService.IsExistsMobileAsync(validateMobileExistsInput.Mobile, validateMobileExistsInput.UserId);
            if (!isExists)
            {
                result.Code = 200;
                result.Message = "验证通过";
            }
            else
            {
                result.Code = 400;
                result.Message = "手机号已经被使用";
            }
            return result;
        }

        /// <summary>
        /// 验证邮箱是否被使用
        /// </summary>
        /// <param name="validateEmailExistsInput"></param>
        /// <returns></returns>
        [HttpPost("ValidateEmailExists")]
        [TubumuAuthorize(Permissions = "用户管理")]
        public async Task<ApiResult> ValidateEmailExists(ValidateEmailExistsInput validateEmailExistsInput)
        {
            var result = new ApiResult();
            var isExists = await _userService.IsExistsEmailAsync(validateEmailExistsInput.Email, validateEmailExistsInput.UserId);
            if (!isExists)
            {
                result.Code = 200;
                result.Message = "验证通过";
            }
            else
            {
                result.Code = 400;
                result.Message = "邮箱已经被使用";
            }
            return result;
        }

        #endregion

        #region 分组

        /// <summary>
        /// 获取分组列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetGroupList")]
        [AllowAnonymous]
        public async Task<ApiResultData<List<Group>>> GetGroupList()
        {
            var groups = await _groupService.GetListInCacheAsync();
            ProjectGroups(ref groups);
            var result = new ApiResultData<List<Group>>
            {
                Code = 200,
                Message = "获取分组列表成功",
                Data = groups,
            };

            return result;
        }

        /// <summary>
        /// 获取分组树
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetGroupTree")]
        [AllowAnonymous]
        public async Task<ApiResultData<List<GroupTreeNode>>> GetGroupTree()
        {
            var tree = await _groupService.GetTreeInCacheAsync();
            var result = new ApiResultData<List<GroupTreeNode>>
            {
                Code = 200,
                Message = "获取分组树成功",
                Data = tree,
            };

            return result;
        }

        /// <summary>
        /// 添加分组
        /// </summary>
        /// <param name="groupInput"></param>
        /// <returns></returns>
        [HttpPost("AddGroup")]
        [TubumuAuthorize(Permissions = "分组管理")]
        public async Task<ApiResult> AddGroup(GroupInput groupInput)
        {
            var result = new ApiResult();
            if (groupInput.GroupId.HasValue)
            {
                // Guid.Empty 也不允许
                result.Code = 400;
                result.Message = "添加分组失败：无需提供参数 GroupId";
                return result;
            }
            groupInput.GroupId = Guid.NewGuid();
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
        [TubumuAuthorize(Permissions = "分组管理")]
        public async Task<ApiResult> EditGroup(GroupInput groupInput)
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
        [TubumuAuthorize(Permissions = "分组管理")]
        public async Task<ApiResult> RemoveGroup(GroupIdInput groupIdInput)
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
        [TubumuAuthorize(Permissions = "分组管理")]
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
        [HttpGet("GetRoleList")]
        public async Task<ApiResultData<List<Role>>> GetRoleList()
        {
            var roles = await _roleService.GetListInCacheAsync();
            var result = new ApiResultData<List<Role>>
            {
                Code = 200,
                Message = "获取角色列表成功",
                Data = roles,
            };

            return result;
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRoleBaseList")]
        public async Task<ApiResultData<List<RoleBase>>> GetRoleBaseList()
        {
            var roles = await _roleService.GetBaseListInCacheAsync();
            var result = new ApiResultData<List<RoleBase>>
            {
                Code = 200,
                Message = "获取角色列表成功",
                Data = roles,
            };

            return result;
        }

        /// <summary>
        /// 保存角色名称
        /// </summary>
        /// <param name="saveRoleNameInput"></param>
        /// <returns></returns>
        [HttpPost("SaveRoleName")]
        [TubumuAuthorize(Permissions = "角色管理")]
        public async Task<ApiResult> SaveRoleName(RoleNameInput saveRoleNameInput)
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
        /// 添加角色
        /// </summary>
        /// <param name="roleInput"></param>
        /// <returns></returns>
        [HttpPost("AddRole")]
        [TubumuAuthorize(Permissions = "角色管理")]
        public async Task<ApiResultData<Role>> AddRole(RoleInput roleInput)
        {
            var result = new ApiResultData<Role>();
            if (roleInput.RoleId.HasValue)
            {
                // Guid.Empty 也不允许
                result.Code = 400;
                result.Message = "添加角色失败：无需提供参数 RoleId";
                return result;
            }

            roleInput.RoleId = Guid.NewGuid();
            if (!await _roleService.SaveAsync(roleInput, ModelState))
            {
                result.Code = 400;
                result.Message = "添加角色失败：" + ModelState.FirstErrorMessage();
                return result;
            }

            // 角色名称不能重复
            var role = await _roleService.GetItemAsync(roleInput.Name);

            result.Code = 200;
            result.Message = "添加角色成功";
            result.Data = role;
            return result;
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="roleInput"></param>
        /// <returns></returns>
        [HttpPost("EditRole")]
        [TubumuAuthorize(Permissions = "角色管理")]
        public async Task<ApiResultData<Role>> EditRole(RoleInput roleInput)
        {
            var result = new ApiResultData<Role>();
            if (roleInput.RoleId.IsNullOrEmpty())
            {
                result.Code = 400;
                result.Message = "编辑角色失败：必须提供参数 RoleId";
                return result;
            }

            if (!await _roleService.SaveAsync(roleInput, ModelState))
            {
                result.Code = 400;
                result.Message = "编辑角色失败：" + ModelState.FirstErrorMessage();
                return result;
            }

            // 角色名称不能重复
            var role = await _roleService.GetItemAsync(roleInput.Name);

            result.Code = 200;
            result.Message = "编辑角色成功";
            result.Data = role;
            return result;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleIdInput"></param>
        /// <returns></returns>
        [HttpPost("RemoveRole")]
        [TubumuAuthorize(Permissions = "角色管理")]
        public async Task<ApiResult> RemoveRole(RoleIdInput roleIdInput)
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
        [TubumuAuthorize(Permissions = "角色管理")]
        public async Task<ApiResult> MoveRole(MoveRoleInput moveRoleInput)
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
        public async Task<ApiResultData<List<PermissionTreeNode>>> GetPermissionTree()
        {
            var tree = await _permissionService.GetTreeInCacheAsync();
            var result = new ApiResultData<List<PermissionTreeNode>>
            {
                Code = 200,
                Message = "获取权限树成功",
                Data = tree,
            };

            return result;
        }

        #endregion

        #region 用户状态

        /// <summary>
        /// 获取用户状态
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserStatuList")]
        public ApiResultData<IEnumerable<KeyValuePair<UserStatus, string>>> GetUserStatuList()
        {
            var list = typeof(UserStatus).GetEnumDictionary<UserStatus>();
            var result = new ApiResultData<IEnumerable<KeyValuePair<UserStatus, string>>>
            {
                Code = 200,
                Message = "获取用户状态列表成功",
                Data = list,
            };

            return result;
        }

        #endregion

        #endregion

        #region Private Methods

        private void ProjectPermissions(ref List<Permission> permissions)
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

        private void ProjectGroups(ref List<Group> groups)
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

        #endregion
    }
}
