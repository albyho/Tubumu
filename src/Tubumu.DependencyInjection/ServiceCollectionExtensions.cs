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
                    // 将类注册为其实现的所有非 ITransientDependency、ISingletonDependency 和 IScopedDependency 的接口的服务
                    foreach (var handlerTypeInfo in interfaceTypeInfos.Where(m => m != typeof(ITransientDependency) && m != typeof(ISingletonDependency) && m != typeof(IScopedDependency)))
                    {
                        if (typeof(ITransientDependency).IsAssignableFrom(type))
                        {
                            services.AddTransient(handlerTypeInfo.AsType(), type.AsType());
                        }
                        else if (typeof(ISingletonDependency).IsAssignableFrom(type))
                        {
                            services.AddSingleton(handlerTypeInfo.AsType(), type.AsType());
                        }
                        else if (typeof(IScopedDependency).IsAssignableFrom(type))
                        {
                            services.AddScoped(handlerTypeInfo.AsType(), type.AsType());
                        }
                    }
                }
                else
                {
                    // 类没有实现非 ITransientDependency、ISingletonDependency 或 IScopedDependency 的其他接口
                    if (typeof(ITransientDependency).IsAssignableFrom(type))
                    {
                        services.AddTransient(type.AsType());
                    }
                    else if (typeof(ISingletonDependency).IsAssignableFrom(type))
                    {
                        services.AddSingleton(type.AsType());
                    }
                    else if (typeof(IScopedDependency).IsAssignableFrom(type))
                    {
                        services.AddScoped(type.AsType());
                    }
                }
            }
        }
    }
}
