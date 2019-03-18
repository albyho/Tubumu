using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OrchardCore.BackgroundTasks;
using OrchardCore.Modules;
using OrchardCore.Modules.Manifest;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.BackgroundTasks;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Mappings;
using Tubumu.Modules.Framework.Models;
using Tubumu.Modules.Framework.Services;
using Tubumu.Modules.Framework.SignalR;
using Tubumu.Modules.Framework.Swagger;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Tubumu.Modules.Framework
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup : StartupBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;
        private readonly ILogger<Startup> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        /// <param name="logger"></param>
        public Startup(
            IConfiguration configuration,
            IHostingEnvironment environment,
            ILogger<Startup> logger)
        {
            _configuration = configuration;
            _environment = environment;
            _logger = logger;
        }

        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services"></param>
        public override void ConfigureServices(IServiceCollection services)
        {
            // Background Service
            services.AddSingleton<IBackgroundTask, IdleBackgroundTask>();

            // Cache
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = _environment.ApplicationName + ":";
            });

            services.AddMemoryCache();

            // Cors
            services.AddCors(options => options.AddPolicy("DefaultPolicy",
                builder => builder.WithOrigins("http://localhost:9090", "http://localhost:8080").AllowAnyMethod().AllowAnyHeader().AllowCredentials())
            // builder => builder.AllowAnyOrigin.AllowAnyMethod().AllowAnyHeader().AllowCredentials())
            );

            // Cookie
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false; // 需保持为 false, 否则 Web API 不会 Set-Cookie 。
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // Session
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.Name = ".Tubumu.Session";
                options.Cookie.HttpOnly = true;
            });

            // HTTP Client
            services.AddHttpClient();

            // ApiBehaviorOptions
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context => new OkObjectResult(new ApiResult
                {
                    Code = 400,
                    Message = context.ModelState.FirstErrorMessage()
                });
            });

            // Authentication
            var registeredServiceDescriptor = services.FirstOrDefault(s => s.Lifetime == ServiceLifetime.Transient && s.ServiceType == typeof(IApplicationModelProvider) && s.ImplementationType == typeof(AuthorizationApplicationModelProvider));
            if (registeredServiceDescriptor != null)
            {
                services.Remove(registeredServiceDescriptor);
            }
            services.AddTransient<IApplicationModelProvider, PermissionAuthorizationApplicationModelProvider>();

            services.AddSingleton<ITokenService, TokenService>();
            var tokenValidationSettings = _configuration.GetSection("TokenValidationSettings").Get<TokenValidationSettings>();
            services.AddSingleton(tokenValidationSettings);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = tokenValidationSettings.ValidIssuer,
                        ValidateIssuer = true,

                        ValidAudience = tokenValidationSettings.ValidAudience,
                        ValidateAudience = true,

                        IssuerSigningKey = SignatureHelper.GenerateSigningKey(tokenValidationSettings.IssuerSigningKey),
                        ValidateIssuerSigningKey = tokenValidationSettings.ValidateLifetime,

                        ValidateLifetime = true,
                        ClockSkew  = TimeSpan.FromSeconds(tokenValidationSettings.ClockSkewSeconds),
                    };

                    // We have to hook the OnMessageReceived event in order to
                    // allow the JWT authentication handler to read the access
                    // token from the query string when a WebSocket or 
                    // Server-Sent Events request comes in.
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            _logger.LogError($"Authentication Failed(OnAuthenticationFailed): {context.Request.Path} Error: {context.Exception}");
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            _logger.LogError($"Authentication Challenge(OnChallenge): {context.Request.Path}");

                            // TODO: (alby)为不同客户端返回不同的内容
                            var result = new ApiUrlResult()
                            {
                                Code = 400,
                                Message = "Authentication Challenge",
                                Url = tokenValidationSettings.LoginUrl,
                            };
                            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result));
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            context.Response.Body.Write(body, 0, body.Length);
                            context.HandleResponse();
                            return Task.CompletedTask;
                        }
                    };
                });

            // JSON Date format
            void JsonSetup(MvcJsonOptions options) => options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            services.Configure((Action<MvcJsonOptions>)JsonSetup);

            // SignalR
            services.AddSignalR();
            services.Replace(ServiceDescriptor.Singleton(typeof(IUserIdProvider), typeof(NameUserIdProvider)));

            // AutoMapper
            services.AddAutoMapper();
            Initalizer.Initialize();

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = _environment.ApplicationName + " API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "权限认证(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                c.AddSecurityRequirement(security);
                c.DescribeAllEnumsAsStrings();
                c.DocumentFilter<HiddenApiDocumentFilter>();
                c.DocumentFilter<PermissionDocumentFilter>();
                IncludeXmlCommentsForModules(c);
                c.OrderActionsBy(m => m.ActionDescriptor.DisplayName);
            });
        }

        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app"></param>
        /// <param name="routes"></param>
        /// <param name="serviceProvider"></param>
        public override void Configure(IApplicationBuilder app, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            app.UseCookiePolicy();
            app.UseSession();
            app.UseCors("DefaultPolicy");
            app.UseAuthentication();

            // Swagger
            var swaggerIndexAssembly = typeof(Startup).Assembly;
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", _environment.ApplicationName + " API v1");
                c.IndexStream = () => swaggerIndexAssembly.GetManifestResourceStream(swaggerIndexAssembly.GetName().Name + ".Swagger>Tubumu.SwaggerUI.Index.html");
            });
        }

        private void IncludeXmlCommentsForModules(SwaggerGenOptions swaggerGenOptions)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var assembly = Assembly.Load(new AssemblyName(_environment.ApplicationName));
            var moduleNames = assembly.GetCustomAttributes<ModuleNameAttribute>().Select(m => m.Name);
            moduleNames.ForEach(m =>
            {
                var commentsFileName = m + ".XML";
                var commentsFilePath = Path.Combine(baseDirectory, commentsFileName);
                swaggerGenOptions.IncludeXmlComments(commentsFilePath);
            });
        }
    }
}
