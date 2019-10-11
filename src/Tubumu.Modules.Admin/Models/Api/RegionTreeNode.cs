using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Tubumu.Modules.Admin.Models.Api
{
    /// <summary>
    /// 区域树节点
    /// </summary>
    public class RegionTreeNode
    {
        /// <summary>
        /// 区域 Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 父 Id
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 父节点 Id 路径
        /// </summary>
        public List<int> ParentIdPath { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 是否有子节点
        /// </summary>
        public bool HasChildren { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<RegionTreeNode> Children { get; set; }

        /// <summary>
        /// 首字拼音首字母
        /// </summary>
        public string Initial { get; set; }

        /// <summary>
        /// 各字拼音首字母
        /// </summary>
        public string Initials { get; set; }

        /// <summary>
        /// 拼音
        /// </summary>
        public string Pinyin { get; set; }

        /// <summary>
        /// 额外
        /// </summary>
        public string Extra { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        public string RegionCode { get; set; }
    }
}
