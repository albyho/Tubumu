using Microsoft.AspNetCore.Authorization;

namespace Tubumu.Modules.Framework.Authorization
{
    /// <summary>
    /// 授权数据
    /// </summary>
    public interface IPermissionAuthorizeData : IAuthorizeData
    {
        /// <summary>
        /// 规则
        /// </summary>
        string Rule { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        string Groups { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        string Permissions { get; set; }
    }
}
