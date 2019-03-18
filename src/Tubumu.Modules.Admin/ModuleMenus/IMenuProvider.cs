﻿using System.Collections.Generic;
using Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.ModuleMenus
{
    /// <summary>
    /// Menu Provider
    /// </summary>
    public interface IMenuProvider
    {
        /// <summary>
        /// 顺序
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 获取模块菜单
        /// </summary>
        /// <returns></returns>
        IEnumerable<ModuleMenu> GetModuleMenus();
    }
}
