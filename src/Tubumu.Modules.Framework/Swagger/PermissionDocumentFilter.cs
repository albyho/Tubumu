using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Tubumu.Modules.Framework.Authorization;
using Tubumu.Modules.Framework.Extensions;

namespace Tubumu.Modules.Framework.Swagger
{
    /// <summary>
    /// PermissionDocumentFilter
    /// </summary>
    public class PermissionDocumentFilter : IDocumentFilter
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
                string extension = null;
                var allowAnonymousAttribute = GetCustomAttribute<AllowAnonymousAttribute>(apiDescription, true);
                if (allowAnonymousAttribute != null)
                {
                    continue;
                }

                // TODO: 目前仅考虑了单个 PermissionAuthorizeAttribute，并且显示不够清晰
                var permissionAuthorizeAttribute = GetCustomAttribute<PermissionAuthorizeAttribute>(apiDescription, true);
                if (permissionAuthorizeAttribute != null)
                {
                    var permissions = permissionAuthorizeAttribute.Permissions;
                    var roles = permissionAuthorizeAttribute.Roles;
                    var groups = permissionAuthorizeAttribute.Groups;
                    extension = permissions;
                    if(!roles.IsNullOrEmpty())
                    {
                        extension = extension.IsNullOrEmpty() ? roles : $"({extension})&&({roles})";
                    } 
                    if (!groups.IsNullOrEmpty())
                    {
                        extension = extension.IsNullOrEmpty() ? groups : (!roles.IsNullOrEmpty() ? $"{extension}&&({groups})" : $"({extension})&&({groups})");
                    }

                    if (!extension.IsNullOrEmpty())
                    {
                        extension = extension.Replace(",", "||");
                        extension = $"[{extension}]";
                    }
                }
                else
                {
                    // TODO: 目前仅考虑了单个 AuthorizeAttribute
                    var authorizeAttribute = GetCustomAttribute<AuthorizeAttribute>(apiDescription, true);
                    if (authorizeAttribute != null)
                    {
                        extension = "[认证]";
                    }
                }

                if (!extension.IsNullOrEmpty())
                {
                    var key = "/" + apiDescription.RelativePath;
                    if (key.Contains("?"))
                    {
                        int idx = key.IndexOf("?", StringComparison.Ordinal);
                        key = key.Substring(0, idx);
                    }

                    if (swaggerDoc.Paths.TryGetValue(key, out var pathItem))
                    {
                        var newKey = key + " " + extension;
                        swaggerDoc.Paths[newKey] = pathItem;
                        swaggerDoc.Paths.Remove(key);
                    }
                }
            }
        }

        private static T GetCustomAttribute<T>(ApiDescription apiDescription, bool inherit) where T : Attribute
        {
            var actionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
            var attribute = actionDescriptor?.ControllerTypeInfo.GetCustomAttribute<T>(inherit);
            if (attribute == null)
            {
                if(apiDescription.TryGetMethodInfo(out var methodInfo))
                {
                    attribute = methodInfo.GetCustomAttribute<T>(inherit);
                }
            }
            return attribute;
        }
    }
}
