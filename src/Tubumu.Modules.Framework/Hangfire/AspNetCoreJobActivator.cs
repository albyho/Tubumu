using System;
using System.Collections.Generic;
using System.Text;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Tubumu.Modules.Framework.Hangfire
{
    public class AspNetCoreJobActivator : JobActivator
    {
        private readonly IServiceProvider _applicationServices;

        public AspNetCoreJobActivator(IServiceProvider applicationServices)
        {
            _applicationServices = applicationServices;
        }

        public virtual object ActivateJob(Type jobType)
        {
            return _applicationServices.GetService(jobType);
        }
    }
}
