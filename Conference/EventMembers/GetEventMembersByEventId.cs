using System;
using System.Threading;
using System.Threading.Tasks;

namespace Conference.EventMembers
{
    public class GetEventMembersByEventId : EventQuery, IQuery<PageModel<EventMemberModel>>
    {
        public GetEventMembersByEventId(Guid eventId) : base(eventId)
        {
        }

        public class Handler : IQueryHandler<GetEventMembersByEventId, PageModel<EventMemberModel>>
        {
            public Handler(EventMemberRepository repository)
            {
                Repository = repository;
            }

            private EventMemberRepository Repository { get; }

            public Task<PageModel<EventMemberModel>> Handle(GetEventMembersByEventId query, CancellationToken cancellationToken)
            {
                return Repository.GetAllForEvent(query, cancellationToken);
            }
        }
    }
}
