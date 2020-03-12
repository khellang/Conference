using System;
using System.Threading;
using System.Threading.Tasks;

namespace Conference.Events
{
    public class GetEventById : IQuery<EventModel>
    {
        public GetEventById(Guid id)
        {
            Id = id;
        }

        private Guid Id { get; }

        public class Handler : IQueryHandler<GetEventById, EventModel>
        {
            public Handler(EventRepository repository)
            {
                Repository = repository;
            }

            private EventRepository Repository { get; }

            public Task<EventModel> Handle(GetEventById query, CancellationToken cancellationToken)
            {
                return Repository.GetById(query.Id, cancellationToken);
            }
        }
    }
}
