using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Tubumu.Modules.Framework.Swagger
{
    public class HiddenApiFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var apiDescription in context.ApiDescriptions)
            {
                var hasHiddenApiAttribute = false;
                var actionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
                if (actionDescriptor?.ControllerTypeInfo.GetCustomAttribute<HiddenApiAttribute>(true) != null)
                {
                    hasHiddenApiAttribute = true;
                }
                if (!hasHiddenApiAttribute && apiDescription.TryGetMethodInfo(out var methodInfo) && methodInfo.GetCustomAttribute<HiddenApiAttribute>(true) != null)
                {
                    hasHiddenApiAttribute = true;
                }
                if (hasHiddenApiAttribute)
                {
                    var key = "/" + apiDescription.RelativePath;
                    if (key.Contains("?"))
                    {
                        int idx = key.IndexOf("?", System.StringComparison.Ordinal);
                        key = key.Substring(0, idx);
                    }
                    swaggerDoc.Paths.Remove(key);
                }
            }
        }
    }
}
