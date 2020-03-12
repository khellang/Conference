using System;

namespace Conference
{
    public class EventQuery : PagedQuery
    {
        public EventQuery(Guid eventId)
        {
            EventId = eventId;
        }

        public Guid EventId { get; set; }
    }
}
