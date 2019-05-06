using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using Tubumu.Modules.Admin.Application.Services;
using Tubumu.Modules.Admin.Domain.Entities;
using Tubumu.Modules.Admin.Domain.Services;
using Tubumu.Modules.Admin.Settings;
using Tubumu.Modules.Admin.SignalR.Hubs;
using Tubumu.Modules.Admin.Sms;
using Tubumu.Modules.Admin.UI.Frontend;
using Tubumu.Modules.Admin.UI.Navigation;
using Tubumu.Modules.Admin.Weixin;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Sms;

namespace Tubumu.Modules.Admin
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup : StartupBase
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        public override void ConfigureServices(IServiceCollection services)
        {
            // MetaData
            services.AddScoped<IModuleMetaDataProvider, MetaData>();

            // Menu
            services.AddScoped<IMenuProvider, Menus>();

            services.AddHttpClient();
            services.AddDbContext<TubumuContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("Tubumu")).ConfigureWarnings(warnings =>
                {
                    warnings.Throw(CoreEventId.IncludeIgnoredWarning);
                    //warnings.Throw(RelationalEventId.QueryClientEvaluationWarning);
                }));

            // Domain Services
            services.AddScoped<IRegionManager, RegionManager>();
            services.AddScoped<IBulletinManager, BulletinManager>();
            services.AddScoped<INotificationManager, NotificationManager>();
            services.AddScoped<IPermissionManager, PermissionManager>();
            services.AddScoped<IRoleManager, RoleManager>();
            services.AddScoped<IGroupManager, GroupManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IMobileUserManager, MobileUserManager>();
            services.AddScoped<IWeixinUserManager, WeixinUserManager>();

            // Application Services
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IBulletinService, BulletinService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMobileUserService, MobileUserService>();
            services.AddScoped<IWeixinUserService, WeixinUserService>();
            services.AddScoped<IAdminUserService, AdminUserService>();

            // Frontend
            services.Configure<FrontendSettings>(_configuration.GetSection("FrontendSettings"));

            // SubMail 短信发送接口及参数配置
            //services.AddSingleton<ISmsSender, SubMailSmsSender>();
            //services.Configure<SubMailSmsSettings>(_configuration.GetSection("SubMailSmsSettings"));

            // SmbBao 短信发送接口及参数配置
            services.AddSingleton<ISmsSender, SmsBaoSmsSender>();
            services.Configure<SmsBaoSmsSettings>(_configuration.GetSection("SmsBaoSmsSettings"));

            // 认证设置
            services.Configure<AuthenticationSettings>(_configuration.GetSection("AuthenticationSettings"));

            // 头像设置
            services.Configure<AvatarSettings>(_configuration.GetSection("AvatarSettings"));

            // 手机验证码设置
            services.Configure<MobileValidationCodeSettings>(_configuration.GetSection("MobileValidationCodeSettings"));

            // 微信设置
            services.Configure<WeixinAppSettings>(_configuration.GetSection("WeixinSettings:App"));
            services.Configure<WeixinMobileEndSettings>(_configuration.GetSection("WeixinSettings:MobileEnd"));
            services.Configure<WeixinWebSettings>(_configuration.GetSection("WeixinSettings:Web"));
        }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app"></param>
        /// <param name="routes"></param>
        /// <param name="serviceProvider"></param>
        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            //string[] controllerNamespaces = new string[] { "Tubumu.Modules.Admin.Controllers" };

            #region View

            routes.MapAreaRoute(
                name: "Admin.View",
                areaName: "Tubumu.Modules.Admin",
                template: "Admin/View",
                defaults: new { controller = "View", action = "View" }
            ); // 无 namespaces

            routes.MapAreaRoute(
                name: "Admin.View.Action",
                areaName: "Tubumu.Modules.Admin",
                template: "{action}",
                defaults: new { controller = "View", action = "Index" }
            ); // 无 namespaces；Login, Index 等无 Controller 前缀

            #endregion

            app.UseSignalR(configure =>
            {
                configure.MapHub<NotificationHub>("/hubs/notificationHub");
            });
        }
    }
}
