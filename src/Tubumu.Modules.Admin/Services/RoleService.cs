using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Admin.Repositories;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Services
{
    public interface IRoleService
    {
        Task<Role> GetItemAsync(Guid roleId);
        Task<Role> GetItemAsync(string name);
        Task<List<RoleBase>> GetBaseListInCacheAsync();
        Task<List<Role>> GetListInCacheAsync();
        Task<Role> SaveAsync(RoleInput roleInput, ModelStateDictionary modelState);
        Task<bool> RemoveAsync(Guid roleId, ModelStateDictionary modelState);
        Task<bool> EditNameAsync(RoleNameInput roleEditNameInput, ModelStateDictionary modelState);
        Task<bool> MoveAsync(Guid roleId, MovingTarget target);
        Task<bool> MoveAsync(int sourceDisplayOrder, int targetDisplayOrder, ModelStateDictionary modelState);
        Task<bool> MoveAsync(Guid sourceRoleId, Guid targetRoleId, ModelStateDictionary modelState);
    }

    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IDistributedCache _cache;
        private const string RoleListCacheKey = "RoleList";

        public RoleService(IRoleRepository repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        #region IRoleService Members

        public async Task<Role> GetItemAsync(Guid roleId)
        {
            return await _repository.GetItemAsync(roleId);
        }

        public async Task<Role> GetItemAsync(string name)
        {
            return await _repository.GetItemAsync(name);
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

        public async Task<Role> SaveAsync(RoleInput roleInput, ModelStateDictionary modelState)
        {
            if (!await ValidateExistsAsync(roleInput, modelState)) return null;
            var result = await _repository.SaveAsync(roleInput, modelState);
            if (result == null)
            {
                modelState.AddModelError("Name", "添加或编辑时保存失败");
            }
            else
            {
                await _cache.RemoveAsync(RoleListCacheKey);
            }

            return result;
        }

        public async Task<bool> RemoveAsync(Guid roleId, ModelStateDictionary modelState)
        {
            var result = await _repository.RemoveAsync(roleId, modelState);
            if (result)
            {
                await _cache.RemoveAsync(RoleListCacheKey);
            }
            return result;
        }

        public async Task<bool> EditNameAsync(RoleNameInput saveRoleNameInput, ModelStateDictionary modelState)
        {
            var result = await _repository.SaveNameAsync(saveRoleNameInput, modelState);
            if (result)
            {
                await _cache.RemoveAsync(RoleListCacheKey);
            }
            return result;
        }

        public async Task<bool> MoveAsync(Guid roleId, MovingTarget target)
        {
            var result = await _repository.MoveAsync(roleId, target);
            if (result)
            {
                await _cache.RemoveAsync(RoleListCacheKey);
            }
            return result;
        }

        public async Task<bool> MoveAsync(int sourceDisplayOrder, int targetDisplayOrder, ModelStateDictionary modelState)
        {
            var result = await _repository.MoveAsync(sourceDisplayOrder, targetDisplayOrder, modelState);
            if (result)
            {
                await _cache.RemoveAsync(RoleListCacheKey);
            }
            return result;
        }

        public async Task<bool> MoveAsync(Guid sourceRoleId, Guid targetRoleId, ModelStateDictionary modelState)
        {
            var result = await _repository.MoveAsync(sourceRoleId, targetRoleId, modelState);
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
            var foundRole = await _repository.GetItemAsync(roleInput.Name);

            if (foundRole != null && roleInput.RoleId != foundRole.RoleId)
            {
                modelState.AddModelError("Name", "角色名称【" + roleInput.Name + "】已经被使用");
                return false;
            }
            return true;
        }

        private async Task<List<Role>> GetListInCacheInternalAsync()
        {
            var roles = await _cache.GetJsonAsync<List<Role>>(RoleListCacheKey);
            if (roles == null)
            {
                roles = await _repository.GetListAsync();
                await _cache.SetJsonAsync<List<Role>>(RoleListCacheKey, roles);
            }
            return roles;
        }

        #endregion
    }
}
