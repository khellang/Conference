using System.Collections.Generic;

namespace Conference.Entities
{
    public class Venue : EventEntity
    {
        public Venue()
        {
            Rooms = new List<Room>();
        }

        public string Name { get; set; } = null!;

        public string? Location { get; set; }

        public ICollection<Room> Rooms { get; }
    }
}