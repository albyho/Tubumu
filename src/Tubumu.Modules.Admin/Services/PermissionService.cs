using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Api;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Admin.Repositories;
using Tubumu.Modules.Core.Extensions;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Services
{
    public interface IPermissionService
    {
        Task<Permission> GetItemAsync(Guid permissionId);
        Task<Permission> GetItemAsync(string name);
        Task<List<Permission>> GetListInCacheAsync();
        Task<List<PermissionTreeNode>> GetTreeInCacheAsync();
        Task<PermissionTreeNode> GetSubTreeInCacheAsync(Guid permissionId);
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
        private const string ListCacheKey = "PermissionList";
        private const string TreeCacheKey = "PermissionTree";

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
        }

        public async Task<List<PermissionTreeNode>> GetTreeInCacheAsync()
        {
            var tree = await GetTreeInCacheInternalAsync();
            return tree;
        }

        public async Task<PermissionTreeNode> GetSubTreeInCacheAsync(Guid permissionId)
        {
            var list = await GetTreeInCacheInternalAsync();
            var node = FindPermissionTreeNode(list, permissionId);
            return node;
        }

        public async Task<bool> SaveAsync(PermissionInput permissionInput, ModelStateDictionary modelState)
        {
            bool result = await _repository.SaveAsync(permissionInput, modelState);
            if (result)
            {
                await RemoveCacheAsync();
            }
            else
            {
                modelState.AddModelError("Name", "添加或编辑时保存失败");

            }
            return result;
        }

        public async Task<bool> SaveAsync(IEnumerable<PermissionInput> permissions, ModelStateDictionary modelState)
        {
            // TODO: (alby)事务处理
            foreach (var item in permissions)
            {
                if (!await _repository.SaveAsync(item, modelState))
                {
                    throw new InvalidOperationException("{0} 权限添加失败: ".FormatWith(item.Name, modelState.FirstErrorMessage()));
                }
            }
            await RemoveCacheAsync();
            return true;
        }

        public async Task<bool> RemoveAsync(Guid permissionId)
        {
            var result = await _repository.RemoveAsync(permissionId);
            if (result)
            {
                await RemoveCacheAsync();
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
                await RemoveCacheAsync();
            }
            return result;
        }

        public async Task<bool> MoveAsync(Guid permissionId, MovingTarget target)
        {
            var result = await _repository.MoveAsync(permissionId, target);
            if (result)
            {
                await _cache.RemoveAsync(ListCacheKey);
            }
            return result;
        }

        #endregion

        #region Private Methods

        private async Task<List<Permission>> GetListInCacheInternalAsync()
        {
            var list = await _cache.GetJsonAsync<List<Permission>>(ListCacheKey);
            if (list == null)
            {
                list = await _repository.GetListAsync();
                await _cache.SetJsonAsync(ListCacheKey, list);
            }
            return list;

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

        private async Task<List<PermissionTreeNode>> GetTreeInCacheInternalAsync()
        {
            var tree = await _cache.GetJsonAsync<List<PermissionTreeNode>>(TreeCacheKey);
            if (tree == null)
            {
                var list = await GetListInCacheInternalAsync();
                tree = new List<PermissionTreeNode>();
                for (var i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (item.Level == 1)
                    {
                        var node = PermissionTreeNodeFromPermisssion(item);
                        node.ParentIdPath = null;
                        tree.Add(node);
                        PermisssionTreeAddChildren(list, node, i);
                    }
                }
                await _cache.SetJsonAsync(TreeCacheKey, tree);
            }
            return tree;
        }

        private PermissionTreeNode FindPermissionTreeNode(List<PermissionTreeNode> list, Guid permissionId)
        {
            PermissionTreeNode result = null;
            foreach (var node in list)
            {
                if (node.Id == permissionId)
                {
                    result = node;
                }
                else if (!node.Children.IsNullOrEmpty())
                {
                    result = FindPermissionTreeNode(node.Children, permissionId);
                }
                if (result != null)
                {
                    break;
                }
            }
            return result;
        }

        private void PermisssionTreeAddChildren(List<Permission> permissions, PermissionTreeNode node, int index)
        {
            for (var i = index + 1; i < permissions.Count; i++)
            {
                var item = permissions[i];
                if (item.ParentId == node.Id)
                {
                    if (node.Children == null)
                    {
                        node.Children = new List<PermissionTreeNode>();
                    }
                    var child = PermissionTreeNodeFromPermisssion(item);
                    // 在父节点的 ParentIdPath 基础上增加 ParentId
                    child.ParentIdPath = node.ParentIdPath != null ? new List<Guid>(node.ParentIdPath) : new List<Guid>(1);
                    child.ParentIdPath.Add(node.Id);
                    node.Children.Add(child);
                    PermisssionTreeAddChildren(permissions, child, i);
                }
            }
        }

        private PermissionTreeNode PermissionTreeNodeFromPermisssion(Permission permission)
        {
            return new PermissionTreeNode
            {
                Id = permission.PermissionId,
                ParentId = permission.ParentId,
                Name = permission.Name,
                Level = permission.Level,
                DisplayOrder = permission.DisplayOrder,
            };
        }

        private async Task RemoveCacheAsync()
        {
            await _cache.RemoveAsync(ListCacheKey);
            await _cache.RemoveAsync(TreeCacheKey);
        }

        #endregion
    }
}
