using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Controllers
{
    public partial class AdminController
    {
        #region 模块权限管理

        /// <summary>
        /// 获取模块权限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetModuleMetaDatas")]
        public async Task<ApiItemResult<MetaDataItem>> GetModuleMetaDatas()
        {
            var permissions = await _permissionService.GetListInCacheAsync();
            ProjectPermissions(permissions);

            var roles = await _roleService.GetListInCacheAsync();
            var groups = await _groupService.GetListInCacheAsync();

            var result = new ApiItemResult<MetaDataItem>
            {
                Code = 200,
                Message = "获取权限列表成功",
                Item = new MetaDataItem
                {
                    Permissions = permissions,
                    Roles = roles,
                    Groups = groups,
                },
            };

            return result;
        }

        /// <summary>
        /// 提取模块权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExtractModuleMetaDatas")]
        [PermissionAuthorize(Permissions = "模块元数据")]
        public async Task<object> ExtractModuleMetaDatas()
        {
            var result = new ApiResult();

            var moduleMetaDataProviders = _moduleMetaDataProviders.OrderBy(m => m.Order);
            foreach (var moduleMetaDataProvider in moduleMetaDataProviders)
            {
                // Permission
                var modulePermissions = moduleMetaDataProvider.GetModulePermissions();
                if (modulePermissions == null) continue;

                var permissionInputs = from p in modulePermissions
                                       select new PermissionInput
                                       {
                                           ModuleName = p.ModuleName,
                                           ParentId = p.ParentId,
                                           PermissionId = p.PermissionId,
                                           Name = p.Name
                                       };
                if (!await _permissionService.SaveAsync(permissionInputs, ModelState))
                {
                    result.Code = 400;
                    result.Message = "提取模块权限失败：" + ModelState.FirstErrorMessage();
                    return result;
                }

                // Role
                var moduleRoles = moduleMetaDataProvider.GetModuleRoles();
                if (moduleRoles == null) continue;

                var roleInputs = from r in moduleRoles
                                 select new RoleInput
                                 {
                                     RoleId = r.RoleId,
                                     Name = r.Name,
                                     PermissionIds = r.PermissionIds,
                                 };

                if (!await _roleService.SaveAsync(roleInputs, ModelState))
                {
                    result.Code = 400;
                    result.Message = "提取模块角色失败：" + ModelState.FirstErrorMessage();
                    return result;
                }

                // Group
                var moduleGroups = moduleMetaDataProvider.GetModuleGroups();
                if (moduleGroups == null) continue;

                var groupInputs = from g in moduleGroups
                                  select new GroupInput
                                  {
                                      ParentId = g.ParentId,
                                      GroupId = g.GroupId,
                                      Name = g.Name,
                                      RoleIds = g.RoleIds,
                                      AvailableRoleIds = g.RoleIds,
                                      PermissionIds = g.PermissionIds,
                                      IsContainsUser = g.IsContainsUser,
                                      IsDisabled = g.IsDisabled,
                                  };

                if (!await _groupService.SaveAsync(groupInputs, ModelState))
                {
                    result.Code = 400;
                    result.Message = "提取模块分组失败：" + ModelState.FirstErrorMessage();
                    return result;
                }
            }

            result.Code = 200;
            result.Message = "提取模块元数据成功";
            return result;
        }

        /// <summary>
        /// 清理模块权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("ClearModulePermissions")]
        [PermissionAuthorize(Permissions = "模块元数据")]
        public async Task<object> ClearModulePermissions()
        {
            //如下实现方式是先从模块中获取全部权限信息，然后从数据库中获取全部权限信息
            //求其差集再删除
            //最佳实现方案当然是将ID传到数据库
            //以delete * from Permission Where PermissionID not in(...)的方式或者 or 的方式

            var result = new ApiResult();

            var modulePermissionIDs = new List<Guid>();
            foreach (var moduleMetaDataProvider in _moduleMetaDataProviders)
            {
                var modulePermissions = moduleMetaDataProvider.GetModulePermissions();
                if (modulePermissions != null)
                    modulePermissionIDs.AddRange(modulePermissions
                        .Select(m => m.PermissionId));
            }

            var perToClear = (await _permissionService.GetListInCacheAsync())
                .OrderByDescending(m => m.Level)
                .Select(m => m.PermissionId)
                .Except(modulePermissionIDs);

            await _permissionService.RemoveAsync(perToClear);

            result.Code = 200;
            result.Message = "清理模块权限成功";
            return result;
        }

        #endregion
    }

    /// <summary>
    /// 元数据记录
    /// </summary>
    public class MetaDataItem
    {
        /// <summary>
        /// 权限列表
        /// </summary>
        public List<Models.Permission> Permissions { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        public List<Models.Role> Roles { get; set; }

        /// <summary>
        /// 分组列表
        /// </summary>
        public List<Models.Group> Groups { get; set; }
    }
}
