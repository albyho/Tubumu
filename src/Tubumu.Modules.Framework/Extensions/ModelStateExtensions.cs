using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Tubumu.Modules.Core.Extensions;

namespace Tubumu.Modules.Framework.Extensions
{
    /// <summary>
    /// ModelStateExtensions
    /// </summary>
    public static class ModelStateExtensions
    {
        /// <summary>
        /// FirstErrorMessage
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static string FirstErrorMessage(this ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
            {
                return String.Empty;
            }
            var item = modelState.FirstOrDefault(m => m.Value.Errors.Count > 0).Value.Errors.First();
            var firstErrorMessage = item.ErrorMessage;
            if (firstErrorMessage.IsNullOrWhiteSpace())
            {
                firstErrorMessage = item.Exception.Message;
            }
            return firstErrorMessage ?? "未指定错误。";
        }
    }
}
