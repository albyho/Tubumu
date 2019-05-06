using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Admin.Domain.Services;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.Models.Api;
using Tubumu.Modules.Admin.Models.Input;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Models;

namespace Tubumu.Modules.Admin.Application.Services
{
    /// <summary>
    /// IGroupService
    /// </summary>
    public interface IGroupService
    {
        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<Group> GetItemAsync(Guid groupId);

        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Group> GetItemAsync(string name);

        /// <summary>
        /// GetListInCacheAsync
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<List<Group>> GetListInCacheAsync(Guid? parentId = null);

        /// <summary>
        /// GetTreeInCacheAsync
        /// </summary>
        /// <returns></returns>
        Task<List<GroupTreeNode>> GetTreeInCacheAsync();

        /// <summary>
        /// GetBasePathAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<List<GroupBase>> GetBasePathAsync(Guid groupId);

        /// <summary>
        /// GetInfoPathAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        Task<List<GroupInfo>> GetInfoPathAsync(Guid groupId);

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="groupInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> SaveAsync(GroupInput groupInput, ModelStateDictionary modelState);

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="groupInputs"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        Task<bool> SaveAsync(IEnumerable<GroupInput> groupInputs, ModelStateDictionary modelState);

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
        /// MoveAsync
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
    /// GroupService
    /// </summary>
    public class GroupService : IGroupService
    {
        private const string ListCacheKey = "GroupList";
        private const string TreeCacheKey = "GroupTree";

        private readonly IGroupManager _manager;
        private readonly IDistributedCache _cache;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="repository"></param>
        public GroupService(IDistributedCache cache, IGroupManager repository)
        {
            _cache = cache;
            _manager = repository;
        }

        #region IGroupService Members

        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public Task<Group> GetItemAsync(Guid groupId)
        {
            return _manager.GetItemAsync(groupId);
        }

        /// <summary>
        /// GetItemAsync
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<Group> GetItemAsync(string name)
        {
            return _manager.GetItemAsync(name);
        }

        /// <summary>
        /// GetListInCacheAsync
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<List<Group>> GetListInCacheAsync(Guid? parentId = null)
        {
            var groups = await GetListInCacheInternalAsync();
            return GererateTree(groups);
        }

        /// <summary>
        /// GetTreeInCacheAsync
        /// </summary>
        /// <returns></returns>
        public Task<List<GroupTreeNode>> GetTreeInCacheAsync()
        {
            return GetTreeInCacheInternalAsync();
        }

        /// <summary>
        /// GetBasePathAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<List<GroupBase>> GetBasePathAsync(Guid groupId)
        {
            var groups = await GetListInCacheInternalAsync();
            return GenerateBasePath(groups, groupId);
        }

        /// <summary>
        /// GetInfoPathAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<List<GroupInfo>> GetInfoPathAsync(Guid groupId)
        {
            var groups = await GetListInCacheInternalAsync();
            return GenerateInfoPath(groups, groupId);
        }

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="groupInput"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> SaveAsync(GroupInput groupInput, ModelStateDictionary modelState)
        {
            if (!await ValidateExistsAsync(groupInput, modelState))
            {
                modelState.AddModelError("Name", $"{groupInput.Name} 已经被使用");
                return false;
            }
            var result = await _manager.SaveAsync(groupInput, modelState);
            if (result)
            {
                RemoveCacheAsync().NoWarning();
            }
            return result;
        }

        /// <summary>
        /// SaveAsync
        /// </summary>
        /// <param name="groups"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> SaveAsync(IEnumerable<GroupInput> groups, ModelStateDictionary modelState)
        {
            // TODO: (alby)事务
            foreach (var item in groups)
            {
                if (!await ValidateExistsAsync(item, modelState))
                {
                    // 已经存在
                    continue;
                }

                if (!await _manager.SaveAsync(item, modelState))
                {
                    throw new InvalidOperationException($"{item.Name} 分组添加失败: {modelState.FirstErrorMessage()}");
                }
            }
            RemoveCacheAsync().NoWarning();
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
            var result = await _manager.RemoveAsync(groupId, modelState);
            if (result)
            {
                RemoveCacheAsync().NoWarning();
            }
            return result;
        }

        /// <summary>
        /// MoveAsync
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="movingTarget"></param>
        /// <returns></returns>
        public async Task<bool> MoveAsync(Guid groupId, MovingTarget movingTarget)
        {
            var result = await _manager.MoveAsync(groupId, movingTarget);
            if (result)
            {
                RemoveCacheAsync().NoWarning();
            }
            return result;
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
            var result = await _manager.MoveAsync(sourceGroupId, targetGroupId, movingLocation, isChild, modelState);
            if (result)
            {
                RemoveCacheAsync().NoWarning();
            }
            return result;
        }

        /// <summary>
        /// MoveAsync
        /// </summary>
        /// <param name="sourceDisplayOrder"></param>
        /// <param name="targetDisplayOrder"></param>
        /// <param name="movingLocation"></param>
        /// <param name="isChild"></param>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public async Task<bool> MoveByDisplayOrderAsync(int sourceDisplayOrder, int targetDisplayOrder, MovingLocation movingLocation, bool? isChild, ModelStateDictionary modelState)
        {
            var result = await _manager.MoveByDisplayOrderAsync(sourceDisplayOrder, targetDisplayOrder, movingLocation, isChild, modelState);
            if (result)
            {
                RemoveCacheAsync().NoWarning();
            }
            return result;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 验证分组名称是否已经被使用
        /// </summary>
        private async Task<bool> ValidateExistsAsync(GroupInput groupInput, ModelStateDictionary modelState)
        {
            var foundGroup = await _manager.GetItemAsync(groupInput.Name);

            if (foundGroup != null && groupInput.GroupId != foundGroup.GroupId)
            {
                modelState.AddModelError("Name", $"分组名称【{groupInput.Name}】已经被使用");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 生成树
        /// </summary>
        /// <param name="source"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private List<Group> GererateTree(List<Group> source, Guid? parentId = null)
        {
            // 算法正确性的前提是记录是按正确顺序排序的

            if (source.IsNullOrEmpty())
                return new List<Group>(0);

            if (!parentId.HasValue)
            {
                return source;
            }

            var list = new List<Group>();
            for (var index = 0; index < source.Count; index++)
            {
                var item = source[index];
                if (list.Count == 0)
                {
                    if (item.GroupId == parentId.Value)
                        list.Add(item);
                }
                else
                {
                    if (item.ParentId == parentId.Value)
                    {
                        list.Add(item);
                        AddChild(source, list, item.GroupId, index);
                    }
                }
            }
            if (list.Count == 0)
                return new List<Group>(0);

            return list;
        }

        private void AddChild(List<Group> source, List<Group> target, Guid parentId, int index)
        {
            // 算法正确性的前提是记录是按正确顺序排序的
            // index 的作用是减少遍历开销

            for (var i = index; i < source.Count; i++)
            {
                var item = source[i];
                if (item.ParentId == parentId)
                {
                    target.Add(item);
                    AddChild(source, target, item.GroupId, i);
                }
            }
        }

        private List<GroupBase> GenerateBasePath(List<Group> source, Guid groupId)
        {
            // 算法正确性的前提是记录是按正确顺序排序的
            if (source.IsNullOrEmpty())
                return new List<GroupBase>(0);

            var list = GeneratePath(source, groupId);

            var baseList = list.Select(m => new GroupBase
            {
                GroupId = m.GroupId,
                Name = m.Name,
                IsContainsUser = m.IsContainsUser,
                IsDisabled = m.IsDisabled,
                IsSystem = m.IsSystem,
            }).ToList();

            return baseList;
        }

        private List<GroupInfo> GenerateInfoPath(List<Group> source, Guid groupId)
        {
            // 算法正确性的前提是记录是按正确顺序排序的

            if (source.IsNullOrEmpty())
                return new List<GroupInfo>(0);

            var list = GeneratePath(source, groupId);

            var infoList = list.Select(m => new GroupInfo
            {
                GroupId = m.GroupId,
                Name = m.Name,
            }).ToList();

            return infoList;
        }

        private List<Group> GeneratePath(List<Group> source, Guid groupId)
        {
            var list = new List<Group>();
            Group item = null;
            int index = -1;
            for (var i = 0; i < source.Count; i++)
            {
                if (source[i].GroupId == groupId)
                {
                    index = i;
                    item = source[i];
                    break;
                }
            }

            if (item == null)
            {
                throw new NullReferenceException($"不存在 Id 为 {groupId} 的分组");
            }

            list.Add(item);

            if (item.ParentId.HasValue)
            {
                AddParent(source, list, item.ParentId.Value, index);
            }

            return list;
        }

        private void AddParent(List<Group> source, List<Group> target, Guid parentId, int index)
        {
            // 算法正确性的前提是记录是按正确顺序排序的
            // index 的作用是减少遍历开销

            for (var i = index - 1; i >= 0; i--)
            {
                var item = source[i];
                if (item.GroupId == parentId)
                {
                    target.Insert(0, item);
                    if (item.ParentId.HasValue)
                    {
                        AddParent(source, target, item.ParentId.Value, i);
                    }
                    break;
                }
            }
        }

        private async Task<List<Group>> GetListInCacheInternalAsync()
        {
            var groups = await _cache.GetJsonAsync<List<Group>>(ListCacheKey);
            if (groups == null)
            {
                groups = await _manager.GetListAsync();
                await _cache.SetJsonAsync(ListCacheKey, groups);
            }
            return groups;
            /*
            if (!_cache.TryGetValue(GroupListCacheKey, out List<Group> groups))
            {
                // Key not in cache, so get data.
                groups = await _manager.GetListAsync();

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromDays(30));

                // Save data in cache.
                _cache.Set(GroupListCacheKey, groups, cacheEntryOptions);
            }

            return groups;
            */
        }

        private async Task<List<GroupTreeNode>> GetTreeInCacheInternalAsync()
        {
            var tree = await _cache.GetJsonAsync<List<GroupTreeNode>>(TreeCacheKey);
            if (tree == null)
            {
                var list = await GetListInCacheInternalAsync();
                tree = new List<GroupTreeNode>();
                for (var i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (item.Level == 1)
                    {
                        var node = GroupTreeNodeFromGroup(item);
                        node.ParentIdPath = null;
                        tree.Add(node);
                        GroupTreeAddChildren(list, node, i);
                    }
                }
                await _cache.SetJsonAsync(TreeCacheKey, tree);
            }
            return tree;
        }

        private void GroupTreeAddChildren(List<Group> groups, GroupTreeNode node, int index)
        {
            // index 参数减少遍历数，前提是数据库中是严格排序的。
            for (var i = index + 1; i < groups.Count; i++)
            {
                var item = groups[i];
                if (item.ParentId == node.Id)
                {
                    if (node.Children == null)
                    {
                        node.Children = new List<GroupTreeNode>();
                    }
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

        private Task RemoveCacheAsync()
        {
            return Task.WhenAll(_cache.RemoveAsync(ListCacheKey), _cache.RemoveAsync(TreeCacheKey));
        }

        #endregion
    }
}
