using System.Collections.Generic;

namespace Tubumu.Modules.Framework.Authorization
{
    public interface IModuleMetaDataProvider
    {
        int Order { get; }

        IEnumerable<Permission> GetModulePermissions();

        IEnumerable<Role> GetModuleRoles();

        IEnumerable<Group> GetModuleGroups();
    }
}
