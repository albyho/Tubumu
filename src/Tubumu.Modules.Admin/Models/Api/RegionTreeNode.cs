using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        /// <summary>
        /// 父 Id
        /// </summary>
        [JsonProperty(PropertyName = "parentId", NullValueHandling = NullValueHandling.Ignore)]
        public int? ParentId { get; set; }

        /// <summary>
        /// 父节点 Id 路径
        /// </summary>
        [JsonProperty(PropertyName = "parentIdPath", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> ParentIdPath { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [JsonProperty(PropertyName = "displayOrder")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        [JsonProperty(PropertyName = "children", NullValueHandling = NullValueHandling.Ignore)]
        public List<GroupTreeNode> Children { get; set; }

        /// <summary>
        /// 首字拼音首字母
        /// </summary>
        [JsonProperty(PropertyName = "initial")]
        public string Initial { get; set; }

        /// <summary>
        /// 各字拼音首字母
        /// </summary>
        [JsonProperty(PropertyName = "initials")]
        public string Initials { get; set; }

        /// <summary>
        /// 拼音
        /// </summary>
        [JsonProperty(PropertyName = "pinyin")]
        public string Pinyin { get; set; }

        /// <summary>
        /// 额外
        /// </summary>
        [JsonProperty(PropertyName = "extra")]
        public string Extra { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        [JsonProperty(PropertyName = "suffix")]
        public string Suffix { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        [JsonProperty(PropertyName = "zipCode")]
        public string ZipCode { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        [JsonProperty(PropertyName = "regionCode")]
        public string RegionCode { get; set; }
    }
}
