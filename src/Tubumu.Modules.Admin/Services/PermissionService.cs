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
using Tubumu.Modules.Framework.Extensions.Object;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Services
{
    public interface IPermissionService
    {
        Task<Permission> GetItemAsync(Guid permissionId);
        Task<Permission> GetItemAsync(string name);
        Task<List<Permission>> GetListInCacheAsync();
        Task<bool> SaveAsync(PermissionInput permissionInput, ModelStateDictionary modelState);
        Task<bool> SaveAsync(IEnumerable<PermissionInput> permissions, ModelStateDictionary modelState);
        Task<bool> RemoveAsync(Guid permissionId);
        Task<bool> RemoveAsync(IEnumerable<Guid> ids);
        Task<bool> MoveAsync(Guid permissionId, MovingTarget target);
    }

    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _repository;
        private readonly IDistributedCache _cache;
        private const string PermissionListCacheKey = "PermissionList";

        public PermissionService(IPermissionRepository repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        #region IPermissionService Members

        public async Task<Permission> GetItemAsync(string name)
        {
            List<Permission> permissions = await GetListInCacheAsync();
            if (!permissions.IsNullOrEmpty())
            {
                return permissions.FirstOrDefault(m => m.Name == name);

            }
            return await _repository.GetItemAsync(name);
        }

        public async Task<Permission> GetItemAsync(Guid permissionId)
        {
            List<Permission> permissions = await GetListInCacheAsync();
            if (!permissions.IsNullOrEmpty())
            {
                return permissions.FirstOrDefault(m => m.PermissionId == permissionId);
            }
            return await _repository.GetItemAsync(permissionId);
        }

        public async Task<List<Permission>> GetListInCacheAsync()
        {
            var permissions = await GetListInCacheInternalAsync();
            return permissions;
            //return ClonePermissions(permissions);
        }

        public async Task<bool> SaveAsync(PermissionInput permissionInput, ModelStateDictionary modelState)
        {
            bool result = await _repository.SaveAsync(permissionInput);
            if (result)
            {
                await _cache.RemoveAsync(PermissionListCacheKey);
            }
            else
            {
                modelState.AddModelError("Name", "添加或编辑时保存失败");

            }
            return result;
        }

        public async Task<bool> SaveAsync(IEnumerable<PermissionInput> permissions, ModelStateDictionary modelState)
        {
            foreach (var per in permissions)
            {
                if (!await _repository.SaveAsync(per))
                {
                    throw new InvalidOperationException("{0} 模块的 {1} 权限添加失败".FormatWith(per.Name));
                }
            }

            await _cache.RemoveAsync(PermissionListCacheKey);
            return true;
        }

        public async Task<bool> RemoveAsync(Guid permissionId)
        {
            var result = await _repository.RemoveAsync(permissionId);
            if (result)
            {
                await _cache.RemoveAsync(PermissionListCacheKey);
            }
            return result;
        }

        public async Task<bool> RemoveAsync(IEnumerable<Guid> ids)
        {
            if (ids == null) return true;

            var result = true;
            foreach (var id in ids)
            {
                var itemResult = await _repository.RemoveAsync(id);
                if (!itemResult)
                {
                    result = false;
                    break;
                }
            }

            if (result)
            {
                await _cache.RemoveAsync(PermissionListCacheKey);
            }
            return result;
        }

        public async Task<bool> MoveAsync(Guid permissionId, MovingTarget target)
        {
            var result = await _repository.MoveAsync(permissionId, target);
            if (result)
            {
                await _cache.RemoveAsync(PermissionListCacheKey);
            }
            return result;
        }

        #endregion

        #region Private Methods

        private List<Permission> ClonePermissions(IEnumerable<Permission> source)
        {
            if (source.IsNullOrEmpty())
            {
                return new List<Permission>(0);
            }

            if (!(source.DeepClone() is IEnumerable<Permission> clone))
            {
                return new List<Permission>(0);
            }

            return clone.ToList();

            //List<Permission> permissions = new List<Permission>();
            //foreach (var item in source)
            //{
            //    permissions.Add(new Permission
            //    {
            //        ParentId = item.ParentId,
            //        ModuleName = item.ModuleName,
            //        PermissionId = item.PermissionId,
            //        Name = item.Name,
            //        Level = item.Level,
            //        DisplayOrder = item.DisplayOrder
            //    });
            //}
            //return permissions;

        }

        public async Task<List<Permission>> GetListInCacheInternalAsync()
        {
            var permissions = await _cache.GetJsonAsync<List<Permission>>(PermissionListCacheKey);
            if (permissions == null)
            {
                permissions = await _repository.GetListAsync();
                await _cache.SetJsonAsync<List<Permission>>(PermissionListCacheKey, permissions);
            }
            return permissions;

            /*
            if (!_cache.TryGetValue(PermissionListCacheKey, out List<Permission> permissions))
            {
                // Key not in cache, so get data.
                permissions = await _repository.GetListAsync();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromDays(30));

                // Save data in cache.
                _cache.Set(PermissionListCacheKey, permissions, cacheEntryOptions);
            }

            return permissions;
            */
        }


        #endregion
    }
}
