using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authorization;
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
                var actionDescriptor = apiDescription.ActionDescriptor as ControllerActionDescriptor;
                // TODO: 目前仅考虑了单个 PermissionAuthorizeAttribute
                var permissionAuthorizeAttribute = actionDescriptor?.ControllerTypeInfo.GetCustomAttribute<PermissionAuthorizeAttribute>(true);
                if (permissionAuthorizeAttribute == null)
                {
                    if(apiDescription.TryGetMethodInfo(out var methodInfo))
                    {
                        permissionAuthorizeAttribute = methodInfo.GetCustomAttribute<PermissionAuthorizeAttribute>(true);
                    }
                }
                if (permissionAuthorizeAttribute != null)
                {
                    var permissions = permissionAuthorizeAttribute.Permissions;
                    var roles = permissionAuthorizeAttribute.Roles;
                    var groups = permissionAuthorizeAttribute.Groups;
                    extension = permissions;
                    if (!extension.IsNullOrEmpty() && !roles.IsNullOrEmpty())
                    {
                        extension = $"({extension})&&({roles})";
                    }
                    if (!extension.IsNullOrEmpty() && !groups.IsNullOrEmpty())
                    {
                        if (!roles.IsNullOrEmpty())
                        {
                            extension = $"({extension})&&({groups})";
                        }
                        else
                        {
                            extension = $"({extension})&&{groups}";
                        }
                    }

                    if (extension != null)
                    {
                        extension = extension.Replace(",", "||");
                        extension = $"[{extension}]";
                    }
                }
                else
                {
                    // TODO: 目前仅考虑了单个 AuthorizeAttribute
                    var authorizeAttribute = actionDescriptor?.ControllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>(true);
                    if (authorizeAttribute == null)
                    {
                        if(apiDescription.TryGetMethodInfo(out var methodInfo))
                        {
                            authorizeAttribute = methodInfo.GetCustomAttribute<AuthorizeAttribute>(true);
                        }
                    }

                    if (authorizeAttribute != null)
                    {
                        extension = "[认证]";
                    }
                }
                if (extension != null)
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
    }
}
