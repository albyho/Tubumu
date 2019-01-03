using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Tubumu.Modules.Admin.Models
{
    /// <summary>
    /// 权限信息
    /// </summary>
    [Serializable]
    public class PermissionBase
    {
        /// <summary>
        /// 权限 Id
        /// </summary>
        [JsonProperty(PropertyName = "permissionId")]
        public Guid PermissionId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        [JsonProperty(PropertyName = "moduleName")]
        public string ModuleName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// 权限信息
    /// </summary>
    [Serializable]
    public class Permission : PermissionBase
    {
        /// <summary>
        /// 所属权限
        /// </summary>
        [DisplayName("所属权限")]
        [JsonProperty(PropertyName = "parentId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? ParentId { set; get; }

        /// <summary>
        /// 层级
        /// </summary>
        [DisplayName("层级")]
        [JsonProperty(PropertyName = "level")]
        public int Level { set; get; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [DisplayName("显示顺序")]
        [JsonProperty(PropertyName = "displayOrder")]
        public int DisplayOrder { set; get; }
    }
}
