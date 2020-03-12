using System;
using System.Threading;
using System.Threading.Tasks;

namespace Conference.SessionFormats
{
    public class GetSessionFormatsByEventId : EventQuery, IQuery<PageModel<SessionFormatModel>>
    {
        public GetSessionFormatsByEventId(Guid eventId) : base(eventId)
        {
        }

        public class Handler : IQueryHandler<GetSessionFormatsByEventId, PageModel<SessionFormatModel>>
        {
            public Handler(SessionFormatRepository repository)
            {
                Repository = repository;
            }

            private SessionFormatRepository Repository { get; }

            public Task<PageModel<SessionFormatModel>> Handle(GetSessionFormatsByEventId query, CancellationToken cancellationToken)
            {
                return Repository.GetAllForEvent(query, cancellationToken);
            }
        }
    }
}
