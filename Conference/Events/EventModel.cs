using NodaTime;

namespace Conference.Events
{
    public class EventModel : EntityModel
    {
        public string Name { get; set; } = null!;

        public Instant StartTime { get; set; }

        public Instant EndTime { get; set; }

        public Instant CfpOpenTime { get; set; }

        public Instant CfpCloseTime { get; set; }

        public string TimeZoneId { get; set; } = null!;
    }
}
