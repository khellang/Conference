using Conference.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Conference.Authorization
{
    public class EventMemberRoleRequirement : IAuthorizationRequirement
    {
        public EventMemberRoleRequirement(EventMemberRole role)
        {
            Role = role;
        }

        public EventMemberRole Role { get; }
    }
}