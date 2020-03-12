using System;
using System.Threading;
using System.Threading.Tasks;

namespace Conference.Venues
{
    public class GetVenuesByEventId : EventQuery, IQuery<PageModel<VenueModel>>
    {
        public GetVenuesByEventId(Guid eventId) : base(eventId)
        {
        }

        public class Handler : IQueryHandler<GetVenuesByEventId, PageModel<VenueModel>>
        {
            public Handler(VenueRepository repository)
            {
                Repository = repository;
            }

            private VenueRepository Repository { get; }

            public Task<PageModel<VenueModel>> Handle(GetVenuesByEventId query, CancellationToken cancellationToken)
            {
                return Repository.GetAllForEvent(query, cancellationToken);
            }
        }
    }
}
