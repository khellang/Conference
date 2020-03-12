using System.Threading;
using System.Threading.Tasks;

namespace Conference.Events
{
    public class GetEvents : PagedQuery, IQuery<PageModel<EventModel>>
    {
        public class Handler : IQueryHandler<GetEvents, PageModel<EventModel>>
        {
            public Handler(EventRepository repository)
            {
                Repository = repository;
            }

            private EventRepository Repository { get; }

            public Task<PageModel<EventModel>> Handle(GetEvents query, CancellationToken cancellationToken)
            {
                return Repository.GetPaged(query, cancellationToken);
            }
        }
    }
}
