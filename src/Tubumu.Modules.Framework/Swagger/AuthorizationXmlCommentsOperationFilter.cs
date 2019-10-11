using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.XPath;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
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
            _xmlNavigator = xmlDoc.CreateNavigator();
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo == null) return;

            // If method is from a constructed generic type, look for comments from the generic type method
            var targetMethod = context.MethodInfo.DeclaringType.IsConstructedGenericType
                ? GetGenericTypeMethodOrNullFor(context.MethodInfo)
                : context.MethodInfo;

            if (targetMethod == null) return;

            var typeMemberName = XmlCommentsNodeNameHelper.GetMemberNameForType(targetMethod.DeclaringType);
            var typeNode = _xmlNavigator.SelectSingleNode(string.Format(MemberXPath, typeMemberName));

            // Apply controller-level tags if any
            if (typeNode != null)
            {
                ApplyResponsesXmlToResponses(operation.Responses, typeNode.Select(ResponsesXPath));
            }

            // Apply controller-level tags if any
            if (typeNode != null)
            {
                ApplyResponsesXmlToResponses(operation.Responses, typeNode.Select(ResponsesXPath));
            }

            var methodMemberName = XmlCommentsNodeNameHelper.GetMemberNameForMethod(targetMethod);
            var methodNode = _xmlNavigator.SelectSingleNode(string.Format(MemberXPath, methodMemberName));

            // Apply method-level tags
            if (methodNode != null)
            {
                ApplyMethodXmlToOperation(operation, methodNode, context.ApiDescription);
                ApplyParamsXmlToActionParameters(operation.Parameters, operation.RequestBody, context.ApiDescription, methodNode);
                ApplyResponsesXmlToResponses(operation.Responses, methodNode.Select(ResponsesXPath)); // will override controller-level response tags
            }

            // Special handling for parameters that are bound to model properties
            ApplyPropertiesXmlToPropertyParameters(operation.Parameters, context.ApiDescription);

            /*
            if (context.MethodInfo == null)
                return;
            MethodInfo method = context.MethodInfo.DeclaringType.IsConstructedGenericType ? this.GetGenericTypeMethodOrNullFor(context.MethodInfo) : context.MethodInfo;
            if (method == null)
                return;
            XPathNavigator methodNode = _xmlNavigator.SelectSingleNode(string.Format("/doc/members/member[@name='{0}']", (object)XmlCommentsMemberNameHelper.GetMemberNameForMethod(method)));
            if (methodNode != null)
            {
                this.ApplyMethodXmlToOperation(operation, methodNode, context.ApiDescription);
                this.ApplyParamsXmlToActionParameters(operation.Parameters, methodNode, context.ApiDescription);
                this.ApplyResponsesXmlToResponses(operation.Responses, methodNode.Select("response"));
            }
            ApplyPropertiesXmlToPropertyParameters(operation.Parameters, context.ApiDescription);
            */
        }

        private MethodInfo GetGenericTypeMethodOrNullFor(MethodInfo constructedTypeMethod)
        {
            var constructedType = constructedTypeMethod.DeclaringType;
            var genericTypeDefinition = constructedType.GetGenericTypeDefinition();

            // Retrieve list of candidate methods that match name and parameter count
            var candidateMethods = genericTypeDefinition.GetMethods()
                .Where(m =>
                {
                    return (m.Name == constructedTypeMethod.Name)
                        && (m.GetParameters().Length == constructedTypeMethod.GetParameters().Length);
                });


            // If inconclusive, just return null
            return (candidateMethods.Count() == 1) ? candidateMethods.First() : null;
        }

        private void ApplyMethodXmlToOperation(OpenApiOperation operation, XPathNavigator methodNode, ApiDescription apiDescription)
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

        private void ApplyParamsXmlToActionParameters(
            IList<OpenApiParameter> parameters,
            OpenApiRequestBody requestBody,
            ApiDescription apiDescription,
            XPathNavigator methodNode)
        {
            if (parameters == null) return;

            foreach (var parameter in parameters)
            {
                // Check for a corresponding action parameter?
                var actionParameter = apiDescription.ActionDescriptor.Parameters
                    .FirstOrDefault(p => parameter.Name.Equals(
                        (p.BindingInfo?.BinderModelName ?? p.Name), StringComparison.OrdinalIgnoreCase));
                if (actionParameter == null) continue;

                var paramNode = methodNode.SelectSingleNode(string.Format(ParamXPath, actionParameter.Name));
                if (paramNode != null)
                    parameter.Description = XmlCommentsTextHelper.Humanize(paramNode.InnerXml);
            }

            if (requestBody != null)
            {
                var actionParameter = apiDescription.ParameterDescriptions
                    .FirstOrDefault(p => IsFromBody(p));

                if (actionParameter != null)
                {
                    var paramNode = methodNode.SelectSingleNode(string.Format(ParamXPath, actionParameter.Name));
                    if (paramNode != null)
                        requestBody.Description = XmlCommentsTextHelper.Humanize(paramNode.InnerXml);
                }
            }
        }

        private void ApplyResponsesXmlToResponses(IDictionary<string, OpenApiResponse> responses, XPathNodeIterator responseNodes)
        {
            while (responseNodes.MoveNext())
            {
                var code = responseNodes.Current.GetAttribute("code", "");
                var response = responses.ContainsKey(code)
                    ? responses[code]
                    : responses[code] = new OpenApiResponse();

                response.Description = XmlCommentsTextHelper.Humanize(responseNodes.Current.InnerXml);
            }
        }

        private void ApplyPropertiesXmlToPropertyParameters(
            IList<OpenApiParameter> parameters,
            ApiDescription apiDescription)
        {
            if (parameters == null) return;

            foreach (var parameter in parameters)
            {
                // Check for a corresponding  API parameter (from ApiExplorer) that's property-bound?
                var propertyParam = apiDescription.ParameterDescriptions
                    .Where(p => p.ModelMetadata?.ContainerType != null && p.ModelMetadata?.PropertyName != null)
                    .FirstOrDefault(p => parameter.Name.Equals(p.Name, StringComparison.OrdinalIgnoreCase));
                if (propertyParam == null) continue;

                var metadata = propertyParam.ModelMetadata;
                var memberInfo = metadata.ContainerType.GetMember(metadata.PropertyName).FirstOrDefault();
                if (memberInfo == null) continue;

                var memberName = XmlCommentsNodeNameHelper.GetNodeNameForMember(memberInfo);
                var memberNode = _xmlNavigator.SelectSingleNode(string.Format(MemberXPath, memberName));
                if (memberNode == null) continue;

                var summaryNode = memberNode.SelectSingleNode(SummaryXPath);
                if (summaryNode != null)
                    parameter.Description = XmlCommentsTextHelper.Humanize(summaryNode.InnerXml);
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

            // TODO: (alby)目前仅考虑了单个 AuthorizationAuthorizeAttribute，并且显示不够清晰
            var permissionAuthorizeAttribute = GetCustomAttribute<PermissionAuthorizeAttribute>(apiDescription, true);
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

        private static bool IsFromBody(ApiParameterDescription apiParameter)
        {
            return apiParameter.Source == BindingSource.Body;
        }
    }
}
