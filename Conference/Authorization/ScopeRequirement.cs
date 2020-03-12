using System;
using Microsoft.AspNetCore.Authorization;

namespace Conference.Authorization
{
    public class ScopeRequirement : IAuthorizationRequirement
    {
        public ScopeRequirement(string scope, string issuer)
        {
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }

        public string Scope { get; }

        public string Issuer { get; }
    }
}