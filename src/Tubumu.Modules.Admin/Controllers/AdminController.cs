using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tubumu.Modules.Admin.Application.Services;
using Tubumu.Modules.Admin.Settings;
using Tubumu.Modules.Admin.UI.Frontend;
using Tubumu.Modules.Admin.UI.Navigation;
using Tubumu.Modules.Framework.Application.Services;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Swagger;

namespace Tubumu.Modules.Admin.Controllers
{
    /// <summary>
    /// Admin Controller
    /// </summary>
    [Route("Api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    [Authorize]
    [HiddenApi]
    public partial class AdminController : ControllerBase
    {
        private readonly IHostingEnvironment _environment;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
        private readonly TokenValidationSettings _tokenValidationSettings;
        private readonly ITokenService _tokenService;
        private readonly FrontendSettings _frontendSettings;
        private readonly AvatarSettings _avatarSettings;
        private const string ValidationCodeKey = "ValidationCode";
        private readonly IUserService _userService;
        private readonly IAdminUserService _adminUserService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly IGroupService _groupService;
        private readonly IRoleService _roleService;
        private readonly IBulletinService _bulletinService;
        private readonly IUserActionLogService _userActionLogService;
        private readonly IEnumerable<IModuleMetaDataProvider> _moduleMetaDataProviders;
        private readonly IEnumerable<IMenuProvider> _menuProviders;
        private readonly ILogger<AdminController> _logger;

        // /Api/Admin/{action}
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="tokenValidationSettings"></param>
        /// <param name="tokenService"></param>
        /// <param name="frontendSettingsOptions"></param>
        /// <param name="avatarSettingsOptions"></param>
        /// <param name="userService"></param>
        /// <param name="adminUserService"></param>
        /// <param name="notificationService"></param>
        /// <param name="permissionService"></param>
        /// <param name="groupService"></param>
        /// <param name="roleService"></param>
        /// <param name="bulletinService"></param>
        /// <param name="userActionLogService"></param>
        /// <param name="moduleMetaDataProviders"></param>
        /// <param name="menuProviders"></param>
        /// <param name="logger"></param>
        public AdminController(
            IHostingEnvironment environment,
            TokenValidationSettings tokenValidationSettings,
            ITokenService tokenService,
            IOptions<FrontendSettings> frontendSettingsOptions,
            IOptions<AvatarSettings> avatarSettingsOptions,
            IUserService userService,
            IAdminUserService adminUserService,
            INotificationService notificationService,
            IPermissionService permissionService,
            IGroupService groupService,
            IRoleService roleService,
            IBulletinService bulletinService,
            IUserActionLogService userActionLogService,
            IEnumerable<IModuleMetaDataProvider> moduleMetaDataProviders,
            IEnumerable<IMenuProvider> menuProviders,
            ILogger<AdminController> logger)
        {
            _environment = environment;
            _tokenValidationSettings = tokenValidationSettings;
            _tokenService = tokenService;
            _frontendSettings = frontendSettingsOptions.Value;
            _avatarSettings = avatarSettingsOptions.Value;
            _userService = userService;
            _adminUserService = adminUserService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _groupService = groupService;
            _roleService = roleService;
            _bulletinService = bulletinService;
            _userActionLogService = userActionLogService;
            _moduleMetaDataProviders = moduleMetaDataProviders;
            _menuProviders = menuProviders;
            _logger = logger;
        }
    }
}
