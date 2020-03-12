using System;

namespace Conference.Entities
{
    public class EventEntity : Entity
    {
        public Guid EventId { get; set; }

        public Event Event { get; set; } = null!;
    }
}
