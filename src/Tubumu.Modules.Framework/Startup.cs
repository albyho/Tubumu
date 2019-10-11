using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrchardCore.BackgroundTasks;
using OrchardCore.Modules.Manifest;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.Newtonsoft;
using Swashbuckle.AspNetCore.SwaggerGen;
using Tubumu.Core.Extensions;
using Tubumu.Core.Extensions.Object;
using Tubumu.Modules.Core.Json;
using Tubumu.Modules.Framework.Application.Services;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.BackgroundTasks;
using Tubumu.Modules.Framework.Extensions;
using Tubumu.Modules.Framework.Mappings;
using Tubumu.Modules.Framework.Models;
using Tubumu.Modules.Framework.RabbitMQ;
using Tubumu.Modules.Framework.SignalR;
using Tubumu.Modules.Framework.Swagger;
using StartupBase = OrchardCore.Modules.StartupBase;

namespace Tubumu.Modules.Framework
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup : StartupBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<Startup> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        /// <param name="logger"></param>
        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment environment,
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
            services.AddControllers()
        .AddNewtonsoftJson();
            // Background Service
            services.AddSingleton<IBackgroundTask, IdleBackgroundTask>();
            services.AddSingleton<IBackgroundTask, NewDayBackgroundTask>();

            // StackExchange.Redis.Extensions
            // https://github.com/imperugo/StackExchange.Redis.Extensions
            var redisConfiguration = _configuration.GetSection("RedisSettings").Get<RedisConfiguration>();
            services.AddSingleton(redisConfiguration);
            services.AddSingleton<IRedisCacheClient, RedisCacheClient>();
            services.AddSingleton<IRedisCacheConnectionPoolManager, RedisCacheConnectionPoolManager>();
            services.AddSingleton<IRedisDefaultCacheClient, RedisDefaultCacheClient>();
            services.AddSingleton<ISerializer, NewtonsoftSerializer>();

            // Cors
            services.AddCors(options => options.AddPolicy("DefaultPolicy",
                builder => builder.WithOrigins("http://localhost:9090", "http://localhost:8080").AllowAnyMethod().AllowAnyHeader().AllowCredentials())
            // builder => builder.AllowAnyOrigin.AllowAnyMethod().AllowAnyHeader().AllowCredentials())
            );

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
            //var registeredServiceDescriptor = services.FirstOrDefault(s => s.Lifetime == ServiceLifetime.Transient && s.ServiceType == typeof(IApplicationModelProvider) && s.ImplementationType == typeof(AuthorizationApplicationModelProvider));
            //if (registeredServiceDescriptor != null)
            //{
            //    services.Remove(registeredServiceDescriptor);
            //}
            //services.AddTransient<IApplicationModelProvider, PermissionAuthorizationApplicationModelProvider>();

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
                        ValidateIssuerSigningKey = true,

                        ValidateLifetime = tokenValidationSettings.ValidateLifetime,
                        ClockSkew = TimeSpan.FromSeconds(tokenValidationSettings.ClockSkewSeconds),
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
                            var result = new ApiResultUrl()
                            {
                                Code = 400,
                                Message = "Authentication Challenge",
                                // TODO: (alby)前端 IsDevelopment 为 true 时不返回 Url
                                Url = _environment.IsProduction() ? tokenValidationSettings.LoginUrl : null,
                            };
                            var body = Encoding.UTF8.GetBytes(result.ToJson());
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            context.Response.Body.Write(body, 0, body.Length);
                            context.HandleResponse();
                            return Task.CompletedTask;
                        }
                    };
                });

            // JSON Date format
            services.AddMvc().
                AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
                option.JsonSerializerOptions.Converters.Add(new DateTimeNullConverter());
                option.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
                option.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                option.JsonSerializerOptions.IgnoreNullValues = true;
            });

            // SignalR
            services.AddSignalR();
            services.Replace(ServiceDescriptor.Singleton(typeof(IUserIdProvider), typeof(NameUserIdProvider)));

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            Initalizer.Initialize();

            // RabbitMQ
            services.AddSingleton<IConnectionPool, ConnectionPool>();
            services.AddSingleton<IChannelPool, ChannelPool>();

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = _environment.ApplicationName + " API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "权限认证(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type =  SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                c.DocumentFilter<HiddenApiDocumentFilter>();
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
        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            app.UseCors("DefaultPolicy");

            // Swagger
            var swaggerIndexAssembly = typeof(Startup).Assembly;
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", _environment.ApplicationName + " API v1");
                c.DefaultModelsExpandDepth(-1);
                //c.IndexStream = () => swaggerIndexAssembly.GetManifestResourceStream(swaggerIndexAssembly.GetName().Name + ".Swagger>Tubumu.SwaggerUI.Index.html");
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
                swaggerGenOptions.IncludeAuthorizationXmlComments(commentsFilePath);
            });
        }
    }
}
