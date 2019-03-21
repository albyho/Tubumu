using System;
using System.Collections.Generic;
using System.Security.Claims;
using Newtonsoft.Json;

namespace Tubumu.Modules.Admin.Models
{
    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// 菜单项(不能包含Children)
        /// </summary>
        Item,
        /// <summary>
        /// 子菜单(不能链接，不能设置为直接访问)
        /// </summary>
        Sub,
        /// <summary>
        /// 菜单组(不能链接，不能设置为直接访问)
        /// </summary>
        Group
    }

    /// <summary>
    /// 模块菜单
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Menu
    {
        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public MenuType Type { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        [JsonProperty(PropertyName = "children", NullValueHandling = NullValueHandling.Ignore)]
        public List<Menu> Children { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        [JsonProperty(PropertyName = "link", NullValueHandling = NullValueHandling.Ignore)]
        public string Link { get; set; } // 运行时计算

        /// <summary>
        /// 是否直接跳转
        /// </summary>
        [JsonProperty(PropertyName = "directly", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Directly { get; set; }

        /// <summary>
        /// 路由名称
        /// </summary>
        public string LinkRouteName { get; set; }

        /// <summary>
        /// 路由值
        /// </summary>
        public object LinkRouteValues { get; set; }

        /// <summary>
        /// 链接 Target
        /// </summary>
        [JsonProperty(PropertyName = "linkTarget", NullValueHandling = NullValueHandling.Ignore)]
        public string LinkTarget { get; set; }

        //权限（只是用于控制菜单显示，并无实际约束能力）
        /// <summary>
        /// 权限
        /// </summary>
        public string Permission { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 验证器（如果有验证器，将忽略 Permission、Role 和 Group）
        /// </summary>
        public Func<ClaimsPrincipal, bool> Validator { get; set; }
    }
}
