using System.Linq;
using Conference.Entities;

namespace Conference.SessionFormats
{
    public class SessionFormatRepository : EventEntityRepository<SessionFormat, SessionFormatModel>
    {
        public SessionFormatRepository(ConferenceContext context) : base(context)
        {
        }

        protected override IOrderedQueryable<SessionFormat> OrderBy(IQueryable<SessionFormat> source, string? field)
        {
            return source.OrderBy(x => x.Name);
        }

        protected override IQueryable<SessionFormatModel> Map(IQueryable<SessionFormat> query)
        {
            return query.Select(format => new SessionFormatModel
            {
                Id = format.Id,
                Name = format.Name,
                Description = format.Description,
                Length = format.Length,
            });
        }
    }
}
