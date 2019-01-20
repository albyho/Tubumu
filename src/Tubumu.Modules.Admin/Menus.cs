using System.Collections.Generic;
using Tubumu.Modules.Admin.Models;
using Tubumu.Modules.Admin.ModuleMenus;
using Tubumu.Modules.Framework.Authorization;

namespace Tubumu.Modules.Admin
{
    /// <summary>
    /// Menus
    /// </summary>
    public class Menus : IMenuProvider
    {
        public int Order => -1000;

        public IEnumerable<ModuleMenu> GetModuleMenus()
        {
            return new List<ModuleMenu>
            {
                /*
                new ModuleMenu
                {
                    Title = "管理首页",
                    Type = 1,
                    Children = new List<ModuleMenu> {
                        new ModuleMenu {
                            Title = "管理首页",
                            LinkRouteName = "Admin.View",
                            LinkRouteValues = new { IsCore = true, Title = "管理首页", Name = "welcome" }, // 因为 Webpack 调试服务器区分大小写，所以统一使用小写
                            Validator = u => u.HasPermission("后台管理"),
                        },
                        new ModuleMenu {
                            Title = "修改基本资料",
                            LinkRouteName = "Admin.ChangeProfile",
                            Validator = u => u.HasPermission("后台修改资料"),
                        },
                        new ModuleMenu {
                            Title = "修改登录密码",
                            LinkRouteName = "Admin.ChangePassword",
                            Validator = u => u.HasPermission("后台修改密码"),
                        },
                        new ModuleMenu {
                            Title = "退出登录",
                            LinkRouteName = "Admin.Api",
                            LinkRouteValues = new { Action = "Logout" }
                        },
                    }
                },*/
                new ModuleMenu
                {
                    Title = "系统管理",
                    Type = ModuleMenuType.Sub,
                    Children = new List<ModuleMenu> {
                         new ModuleMenu{
                             Type = ModuleMenuType.Group,
                             Title ="系统管理",
                             Children = new List<ModuleMenu> {
                                new ModuleMenu{ Title="系统公告", LinkRouteName = "Admin.View", LinkRouteValues = new { IsCore = true, Title = "系统公告", Name = "bulletin" }, Validator = u => u.HasPermission("系统公告")},
                                new ModuleMenu{ Title="通知管理", LinkRouteName = "Admin.View", LinkRouteValues = new { IsCore = true, Title = "通知管理", Name = "notificationmanage", Components = "ckfinder" }, Validator = u => u.HasPermission("通知管理")},
                             }
                         },
                         new ModuleMenu{
                             Type = ModuleMenuType.Group,
                             Title ="模块管理",
                             Children = new List<ModuleMenu> {
                                new ModuleMenu{ Title="模块元数据", LinkRouteName = "Admin.View", LinkRouteValues = new { IsCore = true, Title = "模块元数据", Name = "modulemetadatas" }, Validator = u => u.HasPermission("模块元数据")},
                             }
                         },
                    },
                },
                new ModuleMenu
                {
                    Title = "组织架构管理",
                    Type = ModuleMenuType.Sub,
                    Children = new List<ModuleMenu> {
                         new ModuleMenu{ Title="用户列表", LinkRouteName = "Admin.View", LinkRouteValues = new { IsCore = true, Title = "用户列表", Name = "user", Components = "ckfinder" }, Validator = u => u.HasPermission("用户管理")},
                         new ModuleMenu{ Title="分组列表", LinkRouteName = "Admin.View", LinkRouteValues = new { IsCore = true, Title = "分组列表", Name = "group" }, Validator = u => u.HasPermission("分组管理")},
                         new ModuleMenu{ Title="角色列表", LinkRouteName = "Admin.View", LinkRouteValues = new { IsCore = true, Title = "角色列表", Name = "role" }, Validator = u => u.HasPermission("角色管理")},
                         new ModuleMenu{ Title="权限列表", LinkRouteName = "Admin.View", LinkRouteValues = new { IsCore = true, Title = "权限列表", Name = "permission" }, Validator = u => u.HasPermission("权限管理")},
                    }
                },
            };
        }
    }
}
