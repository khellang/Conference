using System;
using System.Threading;
using System.Threading.Tasks;

namespace Conference.SpeakerProfiles
{
    public class GetSpeakerProfilesByEventId : EventQuery, IQuery<PageModel<SpeakerProfileModel>>
    {
        public GetSpeakerProfilesByEventId(Guid eventId) : base(eventId)
        {
        }

        public class Handler : IQueryHandler<GetSpeakerProfilesByEventId, PageModel<SpeakerProfileModel>>
        {
            public Handler(SpeakerProfileRepository repository)
            {
                Repository = repository;
            }

            private SpeakerProfileRepository Repository { get; }

            public Task<PageModel<SpeakerProfileModel>> Handle(GetSpeakerProfilesByEventId query, CancellationToken cancellationToken)
            {
                return Repository.GetAllForEvent(query, cancellationToken);
            }
        }
    }
}
