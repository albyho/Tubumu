using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Modules;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Cache.Redis;
using Senparc.Weixin.Entities;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.TenPay;
using Senparc.Weixin.WxOpen;

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
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSenparcGlobalServices(_configuration)
                    .AddSenparcWeixinServices(_configuration);
        }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app"></param>
        /// <param name="routes"></param>
        /// <param name="serviceProvider"></param>
        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            ConfigureSenparc(app, routes, serviceProvider);
        }

        private void ConfigureSenparc(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            // 获取服务
            var env = serviceProvider.GetService<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();
            var senparcSetting = serviceProvider.GetService<IOptions<SenparcSetting>>().Value;
            var senparcWeixinSetting = serviceProvider.GetService<IOptions<SenparcWeixinSetting>>().Value;

            // 启动 CO2NET 全局注册，必须！
            var register = RegisterService.Start(env, senparcSetting)
                           //关于 UseSenparcGlobal() 的更多用法见 CO2NET Demo：https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore/Startup.cs
                           .UseSenparcGlobal();

            #region CO2NET 全局配置

            #region 全局缓存配置（按需）

            //当同一个分布式缓存同时服务于多个网站（应用程序池）时，可以使用命名空间将其隔离（非必须）
            register.ChangeDefaultCacheNamespace("DefaultCO2NETCache");

            #region 配置和使用 Redis

            //配置全局使用Redis缓存（按需，独立）
            var redisConfigurationStr = senparcSetting.Cache_Redis_Configuration;
            var useRedis = !string.IsNullOrEmpty(redisConfigurationStr) && redisConfigurationStr != "#{Cache_Redis_Configuration}#"/*默认值，不启用*/;
            if (useRedis)//这里为了方便不同环境的开发者进行配置，做成了判断的方式，实际开发环境一般是确定的，这里的if条件可以忽略
            {
                /* 说明：
                 * 1、Redis 的连接字符串信息会从 Config.SenparcSetting.Cache_Redis_Configuration 自动获取并注册，如不需要修改，下方方法可以忽略
                /* 2、如需手动修改，可以通过下方 SetConfigurationOption 方法手动设置 Redis 链接信息（仅修改配置，不立即启用）
                 */
                Senparc.CO2NET.Cache.Redis.Register.SetConfigurationOption(redisConfigurationStr);

                //以下会立即将全局缓存设置为 Redis
                Senparc.CO2NET.Cache.Redis.Register.UseKeyValueRedisNow();//键值对缓存策略（推荐）
                //Senparc.CO2NET.Cache.Redis.Register.UseHashRedisNow();//HashSet储存格式的缓存策略

                //也可以通过以下方式自定义当前需要启用的缓存策略
                //CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisObjectCacheStrategy.Instance);//键值对
                //CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisHashSetObjectCacheStrategy.Instance);//HashSet
            }
            //如果这里不进行Redis缓存启用，则目前还是默认使用内存缓存 

            #endregion

            #endregion

            Senparc.CO2NET.APM.Config.DataExpire = TimeSpan.FromMinutes(60);//测试APM缓存过期时间（默认情况下可以不用设置）

            #endregion

            #region 微信相关配置

            /* 微信配置开始
             * 
             * 建议按照以下顺序进行注册，尤其须将缓存放在第一位！
             */

            //注册开始

            #region 微信缓存（按需，必须在 register.UseSenparcWeixin() 之前）

            //微信的 Redis 缓存，如果不使用则注释掉（开启前必须保证配置有效，否则会抛错）
            if (useRedis)
            {
                app.UseSenparcWeixinCacheRedis();
            }

            #endregion

            //开始注册微信信息，必须！
            register.UseSenparcWeixin(senparcWeixinSetting, senparcSetting)

            #region 注册公众号或小程序（按需）
                //注册多个公众号或小程序（可注册多个）
                .RegisterWxOpenAccount(senparcWeixinSetting, "主小程序")
                //除此以外，仍然可以在程序任意地方注册公众号或小程序：
                //AccessTokenContainer.Register(appId, appSecret, name);//命名空间：Senparc.Weixin.MP.Containers
            #endregion

            #region 注册微信支付（按需）
                //注册最新微信支付版本（V3）（可注册多个）
                .RegisterTenpayV3(senparcWeixinSetting, "主公众号")//记录到同一个 SenparcWeixinSettingItem 对象中

            #endregion

            ;

            /* 微信配置结束 */
            ;
            #endregion
        }
    }
}
