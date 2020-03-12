using System;
using Microsoft.AspNetCore.Identity;
using NpgsqlTypes;

namespace Conference.Entities
{
    public class User : IdentityUser<Guid>, IEntity
    {
        public string GivenName { get; set; } = null!;

        public string FamilyName { get; set; } = null!;

        public NpgsqlTsVector SearchVector { get; set; } = null!;
    }
}