using System;
using System.Collections.Generic;
using System.Text;
using Tubumu.Modules.Admin.Models;

namespace Tubumu.Modules.Admin.ModuleMenus
{
    public interface IMenuProvider
    {
        int Order { get; }

        IEnumerable<ModuleMenu> GetModuleMenus();
    }
}
