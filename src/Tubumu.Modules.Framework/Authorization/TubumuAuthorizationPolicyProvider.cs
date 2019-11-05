using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Tubumu.Modules.Framework.Authorization.Infrastructure;

namespace Tubumu.Modules.Framework.Authorization
{
    public class TubumuAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        const string PolicyPrefix = "Tubumu:";

        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public TubumuAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
            // doesn't handle all policies (including default policies, etc.) it should fall back to an
            // alternate provider.
            //
            // In this sample, a default authorization policy provider (constructed with options from the 
            // dependency injection container) is used if this custom provider isn't able to handle a given
            // policy name.
            //
            // If a custom policy provider is able to handle all expected policy names then, of course, this
            // fallback pattern is unnecessary.
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        // For ASP.NET Core 3.0
        //public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var policyValue = policyName.Substring(PolicyPrefix.Length);
                var authorizeData = JsonConvert.DeserializeObject<TubumuAuthorizeData>(policyValue);
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new TubumuAuthorizationRequirement(authorizeData));
                return Task.FromResult(policy.Build());
            }

            // If the policy name doesn't match the format expected by this policy provider,
            // try the fallback provider. If no fallback provider is used, this would return 
            // Task.FromResult<AuthorizationPolicy>(null) instead.
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
