using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Tubumu.Modules.Framework.Filters
{
    /// <summary>
    /// DisableFormValueModelBindingAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DisableFormValueModelBindingAttribute : Attribute, IResourceFilter
    {
        /// <inheritdoc/>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (context != null)
            {
                var factories = context.ValueProviderFactories;
                factories.RemoveType<FormValueProviderFactory>();
                factories.RemoveType<JQueryFormValueProviderFactory>();
            }
        }

        /// <inheritdoc/>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            ////throw new NotImplementedException();
        }
    }
}
