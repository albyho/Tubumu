using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// ServiceCollection Extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Auto register services
        /// </summary>
        /// <remarks>为避免全局扫描，仅注册当前执行的程序集的服务</remarks>
        /// <param name="services"></param>
        public static void AddAutoRegisterServices(this IServiceCollection services)
        {
            var classTypes = Assembly.GetExecutingAssembly().
                ExportedTypes.Select(t => t.GetTypeInfo()).
                Where(t => t.IsClass && !t.IsAbstract);

            foreach (var type in classTypes)
            {
                var interfaceTypeInfosTemp = type.ImplementedInterfaces.Select(i => i.GetTypeInfo());
                var interfaceTypeInfos = interfaceTypeInfosTemp as TypeInfo[] ?? interfaceTypeInfosTemp.ToArray();
                if (interfaceTypeInfos.Length > 1)
                {
                    foreach (var handlerType in interfaceTypeInfos.Where(m => m != typeof(ITransientService) && m != typeof(ISingletonService) && m != typeof(IScopedService)))
                    {
                        if (typeof(ITransientService).IsAssignableFrom(type))
                        {
                            services.AddTransient(handlerType.AsType(), type.AsType());
                        }
                        else if (typeof(ISingletonService).IsAssignableFrom(type))
                        {
                            services.AddSingleton(handlerType.AsType(), type.AsType());
                        }
                        else if (typeof(IScopedService).IsAssignableFrom(type))
                        {
                            services.AddScoped(handlerType.AsType(), type.AsType());
                        }
                    }
                }
                else
                {
                    if (typeof(ITransientService).IsAssignableFrom(type))
                    {
                        services.AddTransient(type.AsType());
                    }
                    else if (typeof(ISingletonService).IsAssignableFrom(type))
                    {
                        services.AddSingleton(type.AsType());
                    }
                    else if (typeof(IScopedService).IsAssignableFrom(type))
                    {
                        services.AddScoped(type.AsType());
                    }
                }
            }
        }
    }
}
