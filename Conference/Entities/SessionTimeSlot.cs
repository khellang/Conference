using System;

namespace Conference.Entities
{
    public class SessionTimeSlot
    {
        public Guid SessionId { get; set; }

        public Session Session { get; set; } = null!;

        public Guid TimeSlotId { get; set; }

        public TimeSlot TimeSlot { get; set; } = null!;
    }
}