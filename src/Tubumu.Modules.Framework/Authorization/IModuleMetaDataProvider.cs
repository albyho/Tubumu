using System.Collections.Generic;

namespace Tubumu.Modules.Framework.Authorization
{
    /// <summary>
    /// 模块元数据提供器
    /// </summary>
    public interface IModuleMetaDataProvider
    {
        /// <summary>
        /// 序号
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 获取模块权限列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Permission> GetModulePermissions();

        /// <summary>
        /// 获取模块角色列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Role> GetModuleRoles();

        /// <summary>
        /// 获取模块分组列表
        /// </summary>
        /// <returns></returns>
        IEnumerable<Group> GetModuleGroups();
    }
}
