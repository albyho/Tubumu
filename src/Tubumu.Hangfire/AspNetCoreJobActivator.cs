using System;
using Hangfire;

namespace Tubumu.Hangfire
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
