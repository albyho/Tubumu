using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Tubumu.Modules.Framework.Mappings
{
    /// <summary>
    /// Initalizer
    /// </summary>
    public static class Initalizer
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public static void Initialize()
        {
            // TODO: (alby)全局扫描程序集。考虑其他方式。
            var assembliesToScan = AppDomain.CurrentDomain.GetAssemblies();

            var allTypes = assembliesToScan
                .Where(a => a.GetName().Name != nameof(AutoMapper))
                .SelectMany(a => a.DefinedTypes);

            var profileTypeInfo = typeof(Profile).GetTypeInfo();
            var profiles = allTypes
                .Where(t => profileTypeInfo.IsAssignableFrom(t) && !t.IsAbstract)
                .Select(t => t.AsType());

            Mapper.Initialize(cfg =>
            {
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(profile);
                }
            });
        }
    }
}
