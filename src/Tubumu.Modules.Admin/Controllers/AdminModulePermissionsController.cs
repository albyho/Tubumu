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
        [HttpGet("GetModulePermissions")]
        public async Task<ApiListResult<Models.Permission>> GetModulePermissions()
        {
            var permissions = await _permissionService.GetListInCacheAsync();
            ProjectPermissions(permissions);
            var result = new ApiListResult<Models.Permission>
            {
                Code = 200,
                Message = "获取权限列表成功",
                List = permissions,
            };

            return result;
        }

        /// <summary>
        /// 提取模块权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("ExtractModulePermissions")]
        [PermissionAuthorize(Permissions = "提取权限")]
        public async Task<object> ExtractModulePermissions()
        {
            var result = new ApiResult();

            foreach (var permissionProvider in _permissionProviders)
            {
                var modulePermissions = permissionProvider.GetModulePermissions();
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
            }

            result.Code = 200;
            result.Message = "提取模块权限成功";
            return result;
        }

        /// <summary>
        /// 清理模块权限
        /// </summary>
        /// <returns></returns>
        [HttpGet("ClearModulePermissions")]
        [PermissionAuthorize(Permissions = "清理权限")]
        public async Task<object> ClearModulePermissions()
        {
            //如下实现方式是先从模块中获取全部权限信息，然后从数据库中获取全部权限信息
            //求其差集再删除
            //最佳实现方案当然是将ID传到数据库
            //以delete * from Permission Where PermissionID not in(...)的方式或者 or 的方式

            var result = new ApiResult();

            var modulePermissionIDs = new List<Guid>();
            foreach (var permissionProvider in _permissionProviders)
            {
                var modulePermissions = permissionProvider.GetModulePermissions();
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
}
