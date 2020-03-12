using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Conference.Authorization
{
    public class AuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
            Options = options.Value;
        }

        private AuthorizationOptions Options { get; }

        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);

            if (policy is null)
            {
                policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new ScopeRequirement(policyName, "-- TODO --"))
                    .Build();

                Options.AddPolicy(policyName, policy);
            }

            return policy;
        }
    }
}