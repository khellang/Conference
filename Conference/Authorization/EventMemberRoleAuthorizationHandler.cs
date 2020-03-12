using System;
using System.Threading.Tasks;
using Conference.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Conference.Authorization
{
    public class EventMemberRoleAuthorizationHandler : AuthorizationHandler<EventMemberRoleRequirement, Event>
    {
        public EventMemberRoleAuthorizationHandler(UserManager<User> users)
        {
            Users = users;
        }

        private UserManager<User> Users { get; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EventMemberRoleRequirement requirement, Event resource)
        {
            if (resource is null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            if (resource.Members.Count == 0)
            {
                throw new ArgumentException("Event members can't be empty.", nameof(resource));
            }

            var userIdString = Users.GetUserId(context.User);

            if (userIdString is null)
            {
                return Task.CompletedTask;
            }

            if (Guid.TryParse(userIdString, out var userId))
            {
                foreach (var member in resource.Members)
                {
                    if (member.UserId == userId)
                    {
                        if (HasAccess(member.Role, requirement.Role))
                        {
                            context.Succeed(requirement);
                            return Task.CompletedTask;
                        }

                        return Task.CompletedTask;
                    }
                }
            }

            return Task.CompletedTask;
        }

        private bool HasAccess(EventMemberRole role, EventMemberRole requiredRole)
        {
            return requiredRole switch
            {
                // Surely there must be a better way of writing this :(
                EventMemberRole.Admin => (role == EventMemberRole.Admin),
                EventMemberRole.Organizer => (role == EventMemberRole.Admin || role == EventMemberRole.Organizer),
                EventMemberRole.Reviewer => (role == EventMemberRole.Admin || role == EventMemberRole.Organizer || role == EventMemberRole.Reviewer),
                _ => throw new ArgumentOutOfRangeException(nameof(requiredRole), requiredRole, null)
            };
        }
    }
}