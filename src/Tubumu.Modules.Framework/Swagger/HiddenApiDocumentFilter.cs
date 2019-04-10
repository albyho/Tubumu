using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tubumu.Modules.Framework.Swagger
{
    /// <summary>
    /// HiddenApiDocumentFilter
    /// </summary>
    public class HiddenApiDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// Apply
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var apiDescription in context.ApiDescriptions)
            {
                var ignoreHiddenApiAttribute = GetCustomAttribute<IgnoreHiddenApiAttribute>(apiDescription, true);
                if (ignoreHiddenApiAttribute != null)
                {
                    continue;
                }
                var hiddenApiAttribute = GetCustomAttribute<HiddenApiAttribute>(apiDescription, true);
                if (hiddenApiAttribute != null)
                {
                    var key = "/" + apiDescription.RelativePath;
                    if (key.Contains("?"))
                    {
                        int idx = key.IndexOf("?", StringComparison.Ordinal);
                        key = key.Substring(0, idx);
                    }
                    swaggerDoc.Paths.Remove(key);
                }
            }
        }

        private static T GetCustomAttribute<T>(ApiDescription apiDescription, bool inherit) where T : Attribute
        {
            var actionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
            var attribute = actionDescriptor?.ControllerTypeInfo.GetCustomAttribute<T>(inherit);
            if (attribute == null)
            {
                if (apiDescription.TryGetMethodInfo(out var methodInfo))
                {
                    attribute = methodInfo.GetCustomAttribute<T>(inherit);
                }
            }
            return attribute;
        }
    }
}
