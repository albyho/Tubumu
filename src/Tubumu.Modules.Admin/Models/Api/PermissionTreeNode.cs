using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tubumu.Modules.Admin.Models.Api
{
    /// <summary>
    /// 权限树节点
    /// </summary>
    public class PermissionTreeNode
    {
        /// <summary>
        /// 权限 Id
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
        public List<PermissionTreeNode> Children { get; set; }
    }
}
