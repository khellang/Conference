using System.Collections.Generic;

namespace Conference.Venues
{
    public class VenueModel : EntityModel
    {
        public string Name { get; set; } = null!;

        public string? Location { get; set; }

        public IEnumerable<RoomModel> Rooms { get; set; } = null!;
    }
}