using System.Security.Claims;
using Microsoft.AspNetCore.Builder;

namespace Conference
{
    public static class DeveloperIdentityExtensions
    {
        public static IApplicationBuilder UseDeveloperIdentity(this IApplicationBuilder app, params Claim[] claims)
        {
            return app.Use((ctx, next) =>
            {
                var identity = new ClaimsIdentity(claims, "Developer");

                ctx.User = new ClaimsPrincipal(identity);

                return next();
            });
        }
    }
}
