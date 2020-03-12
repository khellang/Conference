using System.Linq;
using Conference.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conference.EventMembers
{
    public class EventMemberRepository : EventEntityRepository<EventMember, EventMemberModel>
    {
        public EventMemberRepository(ConferenceContext context) : base(context)
        {
        }

        protected override IQueryable<EventMember> Include(IQueryable<EventMember> source)
        {
            return source.Include(x => x.User);
        }

        protected override IOrderedQueryable<EventMember> OrderBy(IQueryable<EventMember> source, string? field)
        {
            return source.OrderBy(x => x.Role).ThenBy(x => x.Email);
        }

        protected override IQueryable<EventMemberModel> Map(IQueryable<EventMember> query)
        {
            return query.Select(member => new EventMemberModel
            {
                Id = member.Id,
                Email = member.Email,
                Role = member.Role,
                User = member.User != null ? new UserModel
                {
                    Id = member.User.Id,
                    GivenName = member.User.GivenName,
                    FamilyName = member.User.FamilyName,
                } : null,
            });
        }
    }
}
