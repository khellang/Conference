using System.Linq;
using Conference.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conference.Venues
{
    public class VenueRepository : EventEntityRepository<Venue, VenueModel>
    {
        public VenueRepository(ConferenceContext context) : base(context)
        {
        }

        protected override IQueryable<Venue> Include(IQueryable<Venue> source)
        {
            return source.Include(x => x.Rooms);
        }

        protected override IOrderedQueryable<Venue> OrderBy(IQueryable<Venue> source, string? field)
        {
            return source.OrderBy(x => x.Name);
        }

        protected override IQueryable<VenueModel> Map(IQueryable<Venue> query)
        {
            return query.Select(venue => new VenueModel
            {
                Id = venue.Id,
                Name = venue.Name,
                Location = venue.Location,
                Rooms = venue.Rooms.OrderBy(x => x.Name).Select(room => new RoomModel
                {
                    Id = room.Id,
                    Name = room.Name,
                    Capacity = room.Capacity,
                }),
            });
        }
    }
}
