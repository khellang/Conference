using System.Linq;
using Conference.Entities;
using NodaTime;

namespace Conference.Events
{
    public class EventRepository : CrudRepository<Event, EventModel>
    {
        public EventRepository(ConferenceContext context, IDateTimeZoneProvider provider) : base(context)
        {
            Provider = provider;
        }

        private IDateTimeZoneProvider Provider { get; }

        protected override IOrderedQueryable<Event> OrderBy(IQueryable<Event> source, string? field)
        {
            return field switch
            {
                "start" => source.OrderBy(x => x.StartTime),
                "end" => source.OrderBy(x => x.EndTime),
                "name" => source.OrderBy(x => x.Name),
                _ => source.OrderBy(x => x.StartTime),
            };
        }

        protected override IQueryable<EventModel> Map(IQueryable<Event> query)
        {
            return query.Select(@event => new EventModel
            {
                Id = @event.Id,
                Name = @event.Name,
                StartTime = @event.StartTime.ToInstant(Provider, @event.TimeZoneId),
                EndTime = @event.EndTime.ToInstant(Provider, @event.TimeZoneId),
                CfpOpenTime = @event.CfpOpenTime.ToInstant(Provider, @event.TimeZoneId),
                CfpCloseTime = @event.CfpCloseTime.ToInstant(Provider, @event.TimeZoneId),
                TimeZoneId = @event.TimeZoneId,
            });
        }
    }
}
