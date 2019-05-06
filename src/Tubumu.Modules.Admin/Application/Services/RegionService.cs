using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Tubumu.Core.Extensions.Object;
using Tubumu.Modules.Admin.Domain.Services;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Api;
using Tubumu.Modules.Framework.Extensions;

namespace Tubumu.Modules.Admin.Application.Services
{
    public interface IRegionService
    {
        Task<string> GetRegionInfoListJsonAsync();

        Task<List<RegionInfo>> GetRegionInfoListAsync(int? parentId = null);

        Task<List<RegionTreeNode>> GetRegiontTreeAsync();

        Task<List<RegionTreeNode>> GetRegiontParentTreeAsync(int[] parentIdPath);

        Task<List<RegionTreeNode>> GetRegiontParentTreeAsync(int regionId);

        Task<List<RegionTreeNode>> GetRegiontTreeChildNodeListAsync(int? parentId);
    }

    public class RegionService : IRegionService
    {
        private const string ListCacheKey = "RegionList";
        private const string TreeCacheKey = "RegionTree";

        private readonly IRegionManager _manager;
        private readonly IDistributedCache _distributedCache;
        private readonly IMemoryCache _memoryCache;

        public RegionService(IRegionManager manager, IDistributedCache distributedCache, IMemoryCache memoryCache)
        {
            _manager = manager;
            _distributedCache = distributedCache;
            _memoryCache = memoryCache;
        }

        public async Task<string> GetRegionInfoListJsonAsync()
        {
            var json = await _distributedCache.GetStringAsync(ListCacheKey);
            if (json == null)
            {
                var list = await _manager.GetRegionInfoListAsync();
                json = list.ToJson();
                await _distributedCache.SetStringAsync(ListCacheKey, json);
            }
            return json;
        }

        public async Task<List<RegionInfo>> GetRegionInfoListAsync(int? parentId = null)
        {
            var list = await GetListInCacheInternalAsync();
            if (parentId.HasValue)
            {
                var subList = list?.Where(m => m.ParentId == parentId).ToList();
                return subList;
            }
            return list;
        }

        public async Task<List<RegionTreeNode>> GetRegiontTreeAsync()
        {
            var tree = await GetTreeInCacheInternalAsync();
            return tree;
        }

        public async Task<List<RegionTreeNode>> GetRegiontParentTreeAsync(int[] parentIdPath)
        {
            // 父级及同级
            var tree = await GetTreeInCacheInternalAsync();
            var newTree = new List<RegionTreeNode>();
            CleanTree(tree, newTree, parentIdPath, 0);
            return newTree;
        }

        public async Task<List<RegionTreeNode>> GetRegiontParentTreeAsync(int regionId)
        {
            // 父级及同级
            var tree = await GetTreeInCacheInternalAsync();
            var self = FindRegion(tree, regionId);
            if (self == null) return new List<RegionTreeNode>(0);
            var newTree = new List<RegionTreeNode>();
            CleanTree(tree, newTree, self.ParentIdPath?.ToArray(), 0);
            return newTree;
        }

        public async Task<List<RegionTreeNode>> GetRegiontTreeChildNodeListAsync(int? parentId)
        {
            // 从 List 缓存中获取，效率更高
            var list = await GetListInCacheInternalAsync();
            var subList = list?.Where(m => m.ParentId == parentId).Select(RegionTreeNodeFromRegion).ToList();
            return subList;
        }

        private RegionTreeNode FindRegion(List<RegionTreeNode> regions, int regionId)
        {
            RegionTreeNode region = null;
            foreach (var node in regions)
            {
                if (node.Id == regionId)
                {
                    region = node;
                    break;
                }
                else if (node.HasChildren && node.Children != null && node.Children.Count > 0)
                {
                    var child = FindRegion(node.Children, regionId);
                    if (child != null)
                    {
                        region = child;
                        break;
                    }
                }
            }

            return region;
        }

        #region Private Methods

        private async Task<List<RegionInfo>> GetListInCacheInternalAsync()
        {
            /*
            var list = await _distributedCache.GetJsonAsync<List<RegionInfo>>(ListCacheKey);
            if (list == null)
            {
                list = await _manager.GetRegionInfoListAsync();
                await _distributedCache.SetJsonAsync<List<RegionInfo>>(ListCacheKey, list);
            }
            return list;
            */

            if (!_memoryCache.TryGetValue(ListCacheKey, out List<RegionInfo> list))
            {
                // Key not in cache, so get data.
                list = await _distributedCache.GetJsonAsync<List<RegionInfo>>(ListCacheKey);
                if (list == null)
                {
                    list = await _manager.GetRegionInfoListAsync();
                    await _distributedCache.SetJsonAsync(ListCacheKey, list);
                }

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromDays(30));

                // Save data in cache.
                _memoryCache.Set(ListCacheKey, list, cacheEntryOptions);
            }

            return list;
        }

        private async Task<List<RegionTreeNode>> GetTreeInCacheInternalAsync()
        {
            /*
            var tree = await _distributedCache.GetJsonAsync<List<RegionTreeNode>>(TreeCacheKey);
            if (tree == null)
            {
                var list = await GetListInCacheInternalAsync();
                tree = new List<RegionTreeNode>();
                for (var i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (!item.ParentId.HasValue)
                    {
                        var node = RegionTreeNodeFromRegion(item);
                        node.ParentIdPath = null;
                        tree.Add(node);
                        RegionTreeAddChildren(list, node);
                    }
                }
                await _distributedCache.SetJsonAsync<List<RegionTreeNode>>(TreeCacheKey, tree);
            }
            return tree;
            */

            if (!_memoryCache.TryGetValue(TreeCacheKey, out List<RegionTreeNode> tree))
            {
                var list = await GetListInCacheInternalAsync();
                tree = new List<RegionTreeNode>();
                for (var i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (!item.ParentId.HasValue)
                    {
                        var node = RegionTreeNodeFromRegion(item);
                        node.ParentIdPath = null;
                        tree.Add(node);
                        RegionTreeAddChildren(list, node);
                    }
                }
                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromDays(30));

                // Save data in cache.
                _memoryCache.Set(TreeCacheKey, tree, cacheEntryOptions);
            }

            return tree;
        }

        private void RegionTreeAddChildren(List<RegionInfo> regions, RegionTreeNode node)
        {
            for (var i = 0; i < regions.Count; i++)
            {
                var item = regions[i];
                if (item.ParentId == node.Id)
                {
                    if (node.Children == null)
                    {
                        node.Children = new List<RegionTreeNode>();
                    }
                    var child = RegionTreeNodeFromRegion(item);
                    // 在父节点的 ParentIdPath 基础上增加 ParentId
                    child.ParentIdPath = node.ParentIdPath != null ? new List<int>(node.ParentIdPath) : new List<int>(1);
                    child.ParentIdPath.Add(node.Id);
                    node.Children.Add(child);
                    RegionTreeAddChildren(regions, child);
                }
            }
        }

        private RegionTreeNode RegionTreeNodeFromRegion(RegionInfo region)
        {
            return new RegionTreeNode
            {
                Id = region.RegionId,
                ParentId = region.ParentId,
                Name = region.Name,
                DisplayOrder = region.DisplayOrder,
                Initial = region.Initial,
                Initials = region.Initials,
                Pinyin = region.Pinyin,
                Extra = region.Extra,
                Suffix = region.Suffix,
                ZipCode = region.ZipCode,
                RegionCode = region.RegionCode,
                HasChildren = region.HasChildren,
            };
        }

        private RegionTreeNode RegionTreeNodeClone(RegionTreeNode node)
        {
            return new RegionTreeNode
            {
                Id = node.Id,
                ParentId = node.ParentId,
                Name = node.Name,
                DisplayOrder = node.DisplayOrder,
                Initial = node.Initial,
                Initials = node.Initials,
                Pinyin = node.Pinyin,
                Extra = node.Extra,
                Suffix = node.Suffix,
                ZipCode = node.ZipCode,
                RegionCode = node.RegionCode,
                HasChildren = node.HasChildren,
                Children = node.Children,
            };
        }

        private void CleanTree(List<RegionTreeNode> source, List<RegionTreeNode> newTree, int[] parentIdPath, int index)
        {
            if (parentIdPath != null && index > parentIdPath.Length - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            foreach (var node in source)
            {
                var newNode = RegionTreeNodeClone(node);
                newTree.Add(newNode);
                if (parentIdPath == null || node.Id != parentIdPath[index])
                {
                    // 顶级节点或非本父节点
                    newNode.Children = null;
                }
                else if (index < parentIdPath.Length - 1)
                {
                    newNode.Children = new List<RegionTreeNode>();
                    // 继续清理下一层
                    CleanTree(node.Children, newNode.Children, parentIdPath, index + 1);
                }
            }
        }

        #endregion
    }
}
