using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Admin.Domain.Services;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Application.Services
{
    public interface IRoleService
    {
        Task<Role> GetItemAsync(Guid roleId);
        Task<Role> GetItemAsync(string name);
        Task<List<RoleBase>> GetBaseListInCacheAsync();
        Task<List<Role>> GetListInCacheAsync();
        Task<bool> SaveAsync(RoleInput roleInput, ModelStateDictionary modelState);
        Task<bool> SaveAsync(IEnumerable<RoleInput> roleInputs, ModelStateDictionary modelState);
        Task<bool> RemoveAsync(Guid roleId, ModelStateDictionary modelState);
        Task<bool> EditNameAsync(RoleNameInput roleEditNameInput, ModelStateDictionary modelState);
        Task<bool> MoveAsync(Guid roleId, MovingTarget target);
        Task<bool> MoveAsync(int sourceDisplayOrder, int targetDisplayOrder, ModelStateDictionary modelState);
        Task<bool> MoveAsync(Guid sourceRoleId, Guid targetRoleId, ModelStateDictionary modelState);
    }

    public class RoleService : IRoleService
    {
        private const string RoleListCacheKey = "RoleList";

        private readonly IRoleManager _manager;
        private readonly IDistributedCache _cache;

        public RoleService(IRoleManager manager, IDistributedCache cache)
        {
            _manager = manager;
            _cache = cache;
        }

        #region IRoleService Members

        public async Task<Role> GetItemAsync(Guid roleId)
        {
            return await _manager.GetItemAsync(roleId);
        }

        public async Task<Role> GetItemAsync(string name)
        {
            return await _manager.GetItemAsync(name);
        }

        public async Task<List<RoleBase>> GetBaseListInCacheAsync()
        {
            var roles = await GetListInCacheInternalAsync();
            var roleBases = roles.Select(m => new RoleBase
            {
                RoleId = m.RoleId,
                Name = m.Name,
                IsSystem = m.IsSystem,
                DisplayOrder = m.DisplayOrder,
            }).ToList();
            return roleBases;
        }

        public async Task<List<Role>> GetListInCacheAsync()
        {
            var roles = await GetListInCacheInternalAsync();
            return roles;
        }

        public async Task<bool> SaveAsync(RoleInput roleInput, ModelStateDictionary modelState)
        {
            if (!await ValidateExistsAsync(roleInput, modelState))
            {
                modelState.AddModelError("Name", $"{roleInput.Name} 已经被使用");
                return false;
            }
            var result = await _manager.SaveAsync(roleInput, modelState);
            if (!result)
            {
                modelState.AddModelError("Name", "添加或编辑时保存失败");
            }
            else
            {
                await _cache.RemoveAsync(RoleListCacheKey);
            }
            return result;
        }

        public async Task<bool> SaveAsync(IEnumerable<RoleInput> roles, ModelStateDictionary modelState)
        {
            // TODO: (alby)事务
            foreach (var item in roles)
            {
                if (!await ValidateExistsAsync(item, modelState))
                {
                    // 已经存在
                    continue;
                }
                if (!await _manager.SaveAsync(item, modelState))
                {
                    throw new InvalidOperationException($"{item.Name} 角色添加失败: {modelState.FirstErrorMessage()}");
                }
            }
            await _cache.RemoveAsync(RoleListCacheKey);
            return true;
        }


        public async Task<bool> RemoveAsync(Guid roleId, ModelStateDictionary modelState)
        {
            var result = await _manager.RemoveAsync(roleId, modelState);
            if (result)
            {
                await _cache.RemoveAsync(RoleListCacheKey);
            }
            return result;
        }

        public async Task<bool> EditNameAsync(RoleNameInput saveRoleNameInput, ModelStateDictionary modelState)
        {
            var result = await _manager.SaveNameAsync(saveRoleNameInput, modelState);
            if (result)
            {
                await _cache.RemoveAsync(RoleListCacheKey);
            }
            return result;
        }

        public async Task<bool> MoveAsync(Guid roleId, MovingTarget target)
        {
            var result = await _manager.MoveAsync(roleId, target);
            if (result)
            {
                await _cache.RemoveAsync(RoleListCacheKey);
            }
            return result;
        }

        public async Task<bool> MoveAsync(int sourceDisplayOrder, int targetDisplayOrder, ModelStateDictionary modelState)
        {
            var result = await _manager.MoveAsync(sourceDisplayOrder, targetDisplayOrder, modelState);
            if (result)
            {
                await _cache.RemoveAsync(RoleListCacheKey);
            }
            return result;
        }

        public async Task<bool> MoveAsync(Guid sourceRoleId, Guid targetRoleId, ModelStateDictionary modelState)
        {
            var result = await _manager.MoveAsync(sourceRoleId, targetRoleId, modelState);
            if (result)
            {
                await _cache.RemoveAsync(RoleListCacheKey);
            }
            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 验证角色名称是否已经被使用
        /// </summary>
        private async Task<bool> ValidateExistsAsync(RoleInput roleInput, ModelStateDictionary modelState)
        {
            var foundRole = await _manager.GetItemAsync(roleInput.Name);

            if (foundRole != null && roleInput.RoleId != foundRole.RoleId)
            {
                modelState.AddModelError("Name", $"角色名称【{roleInput.Name}】已经被使用");
                return false;
            }
            return true;
        }

        private async Task<List<Role>> GetListInCacheInternalAsync()
        {
            var roles = await _cache.GetJsonAsync<List<Role>>(RoleListCacheKey);
            if (roles == null)
            {
                roles = await _manager.GetListAsync();
                await _cache.SetJsonAsync(RoleListCacheKey, roles);
            }
            return roles;
        }

        #endregion
    }
}
