using System;

namespace Conference.Entities
{
    public class Room : Entity
    {
        public string Name { get; set; } = null!;

        public int? Capacity { get; set; }

        public Guid VenueId { get; set; }

        public Venue Venue { get; set; } = null!;
    }
}