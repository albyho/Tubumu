using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tubumu.Modules.Admin.Models.Api
{
    /// <summary>
    /// 用户分组树节点
    /// </summary>
    public class GroupTreeNode
    {
        /// <summary>
        /// 分组 Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 父 Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 父节点 Id 路径
        /// </summary>
        public List<Guid> ParentIdPath { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 层级(从 1 开始)
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<GroupTreeNode> Children { get; set; }

        /// <summary>
        /// 是否允许包含用户
        /// </summary>
        public bool IsContainsUser { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// 是否是系统分组
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// 分组包含的角色
        /// </summary>
        public IEnumerable<RoleBase> Roles { get; set; }

        /// <summary>
        /// 分组可用的角色(该分组的用户的主要角色s只能使用 AvailableRoles)
        /// </summary>
        public IEnumerable<RoleBase> AvailableRoles { get; set; }

        /// <summary>
        /// 分组拥有的权限
        /// </summary>
        public IEnumerable<PermissionBase> Permissions { get; set; }
    }
}
