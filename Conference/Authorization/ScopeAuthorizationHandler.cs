using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Conference.Authorization
{
    public class ScopeAuthorizationHandler : AuthorizationHandler<ScopeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeRequirement requirement)
        {
            var claim = context.User.FindFirst(c =>
                string.Equals(c.Issuer, requirement.Issuer) &&
                string.Equals(c.Type, "scope"));

            if (claim is null)
            {
                return Task.CompletedTask;
            }

            var scopes = claim.Value.Split(' ');

            foreach (var scope in scopes)
            {
                if (string.Equals(scope, requirement.Scope))
                {
                    context.Succeed(requirement);
                    break;
                }
            }

            return Task.CompletedTask;
        }
    }
}