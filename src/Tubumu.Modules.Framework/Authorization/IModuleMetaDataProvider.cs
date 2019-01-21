using System.Collections.Generic;

namespace Tubumu.Modules.Framework.Authorization
{
    /// <summary>
    /// 模块元数据提供器
    /// </summary>
    public interface IModuleMetaDataProvider
    {
        int Order { get; }

        IEnumerable<Permission> GetModulePermissions();

        IEnumerable<Role> GetModuleRoles();

        IEnumerable<Group> GetModuleGroups();
    }
}
