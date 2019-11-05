using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.XPath;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Tubumu.Core.Extensions;
using Tubumu.Modules.Framework.Authorization;

namespace Tubumu.Modules.Framework.Swagger
{
    public class AuthorizationXmlCommentsOperationFilter : IOperationFilter
    {
        private const string MemberXPath = "/doc/members/member[@name='{0}']";
        private const string SummaryXPath = "summary";
        private const string RemarksXPath = "remarks";
        private const string ParamXPath = "param[@name='{0}']";
        private const string ResponsesXPath = "response";
        private readonly XPathNavigator _xmlNavigator;

        public AuthorizationXmlCommentsOperationFilter(XPathDocument xmlDoc)
        {
            this._xmlNavigator = xmlDoc.CreateNavigator();
        }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (context.MethodInfo == (MethodInfo)null)
                return;
            MethodInfo method = context.MethodInfo.DeclaringType.IsConstructedGenericType ? this.GetGenericTypeMethodOrNullFor(context.MethodInfo) : context.MethodInfo;
            if (method == (MethodInfo)null)
                return;
            XPathNavigator methodNode = this._xmlNavigator.SelectSingleNode(string.Format("/doc/members/member[@name='{0}']", (object)XmlCommentsMemberNameHelper.GetMemberNameForMethod(method)));
            if (methodNode != null)
            {
                this.ApplyMethodXmlToOperation(operation, methodNode, context.ApiDescription);
                this.ApplyParamsXmlToActionParameters(operation.Parameters, methodNode, context.ApiDescription);
                this.ApplyResponsesXmlToResponses(operation.Responses, methodNode.Select("response"));
            }
            this.ApplyPropertiesXmlToPropertyParameters(operation.Parameters, context.ApiDescription);
        }

        private MethodInfo GetGenericTypeMethodOrNullFor(MethodInfo constructedTypeMethod)
        {
            IEnumerable<MethodInfo> source = ((IEnumerable<MethodInfo>)constructedTypeMethod.DeclaringType.GetGenericTypeDefinition().GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>)(m =>
          {
              if (m.Name == constructedTypeMethod.Name)
                  return m.GetParameters().Length == constructedTypeMethod.GetParameters().Length;
              return false;
          }));
            if (source.Count<MethodInfo>() != 1)
                return (MethodInfo)null;
            return source.First<MethodInfo>();
        }

        private void ApplyMethodXmlToOperation(Operation operation, XPathNavigator methodNode, ApiDescription apiDescription)
        {
            XPathNavigator xpathNavigator1 = methodNode.SelectSingleNode("summary");
            operation.Summary = GetMemberPermission(apiDescription);
            if (xpathNavigator1 != null)
                operation.Summary += XmlCommentsTextHelper.Humanize(xpathNavigator1.InnerXml);
            XPathNavigator xpathNavigator2 = methodNode.SelectSingleNode("remarks");
            if (xpathNavigator2 == null)
                return;
            operation.Description = XmlCommentsTextHelper.Humanize(xpathNavigator2.InnerXml);
        }

        private void ApplyParamsXmlToActionParameters(IList<IParameter> parameters, XPathNavigator methodNode, ApiDescription apiDescription)
        {
            if (parameters == null)
                return;
            foreach (IParameter parameter1 in (IEnumerable<IParameter>)parameters)
            {
                IParameter parameter = parameter1;
                ParameterDescriptor parameterDescriptor = apiDescription.ActionDescriptor.Parameters.FirstOrDefault<ParameterDescriptor>((Func<ParameterDescriptor, bool>)(p => parameter.Name.Equals(p.BindingInfo?.BinderModelName ?? p.Name, StringComparison.OrdinalIgnoreCase)));
                if (parameterDescriptor != null)
                {
                    XPathNavigator xpathNavigator = methodNode.SelectSingleNode(string.Format("param[@name='{0}']", (object)parameterDescriptor.Name));
                    if (xpathNavigator != null)
                        parameter.Description = XmlCommentsTextHelper.Humanize(xpathNavigator.InnerXml);
                }
            }
        }

        private void ApplyResponsesXmlToResponses(IDictionary<string, Response> responses, XPathNodeIterator responseNodes)
        {
            while (responseNodes.MoveNext())
            {
                string attribute = responseNodes.Current.GetAttribute("code", "");
                (responses.ContainsKey(attribute) ? responses[attribute] : (responses[attribute] = new Response())).Description = XmlCommentsTextHelper.Humanize(responseNodes.Current.InnerXml);
            }
        }

        private void ApplyPropertiesXmlToPropertyParameters(IList<IParameter> parameters, ApiDescription apiDescription)
        {
            if (parameters == null)
                return;
            foreach (IParameter parameter1 in (IEnumerable<IParameter>)parameters)
            {
                IParameter parameter = parameter1;
                ApiParameterDescription parameterDescription = apiDescription.ParameterDescriptions.Where<ApiParameterDescription>((Func<ApiParameterDescription, bool>)(p =>
               {
                   if (p.ModelMetadata?.ContainerType != (Type)null)
                       return p.ModelMetadata?.PropertyName != null;
                   return false;
               })).FirstOrDefault<ApiParameterDescription>((Func<ApiParameterDescription, bool>)(p => parameter.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase)));
                if (parameterDescription != null)
                {
                    ModelMetadata modelMetadata = parameterDescription.ModelMetadata;
                    MemberInfo memberInfo = ((IEnumerable<MemberInfo>)modelMetadata.ContainerType.GetMember(modelMetadata.PropertyName)).FirstOrDefault<MemberInfo>();
                    if (!(memberInfo == (MemberInfo)null))
                    {
                        XPathNavigator xpathNavigator1 = this._xmlNavigator.SelectSingleNode(string.Format("/doc/members/member[@name='{0}']", (object)XmlCommentsMemberNameHelper.GetMemberNameForMember(memberInfo)));
                        if (xpathNavigator1 != null)
                        {
                            XPathNavigator xpathNavigator2 = xpathNavigator1.SelectSingleNode("summary");
                            if (xpathNavigator2 != null)
                                parameter.Description = XmlCommentsTextHelper.Humanize(xpathNavigator2.InnerXml);
                        }
                    }
                }
            }
        }

        private string GetMemberPermission(ApiDescription apiDescription)
        {
            string extension = string.Empty;
            var allowAnonymousAttribute = GetCustomAttribute<AllowAnonymousAttribute>(apiDescription, true);
            if (allowAnonymousAttribute != null)
            {
                return extension;
            }

            // TODO: (alby)目前仅考虑了单个 TubumuAuthorizeAttribute，并且显示不够清晰
            var permissionAuthorizeAttribute = GetCustomAttribute<TubumuAuthorizeAttribute>(apiDescription, true);
            if (permissionAuthorizeAttribute != null)
            {
                var permissions = permissionAuthorizeAttribute.Permissions;
                var roles = permissionAuthorizeAttribute.Roles;
                var groups = permissionAuthorizeAttribute.Groups;
                extension = permissions;
                if (!roles.IsNullOrEmpty())
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
                // TODO: (alby)目前仅考虑了单个 AuthorizeAttribute
                var authorizeAttribute = GetCustomAttribute<AuthorizeAttribute>(apiDescription, true);
                if (authorizeAttribute != null)
                {
                    extension = "[认证]";
                }
            }

            return !extension.IsNullOrWhiteSpace() ? extension + " " : "";
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
