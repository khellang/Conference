using System;
using System.Threading;
using System.Threading.Tasks;

namespace Conference.Submissions
{
    public class GetSubmissionsByEventId : EventQuery, IQuery<PageModel<SubmissionModel>>
    {
        public GetSubmissionsByEventId(Guid eventId) : base(eventId)
        {
        }

        public class Handler : IQueryHandler<GetSubmissionsByEventId, PageModel<SubmissionModel>>
        {
            public Handler(SubmissionRepository repository)
            {
                Repository = repository;
            }

            private SubmissionRepository Repository { get; }

            public Task<PageModel<SubmissionModel>> Handle(GetSubmissionsByEventId query, CancellationToken cancellationToken)
            {
                return Repository.GetAllForEvent(query, cancellationToken);
            }
        }
    }
}
