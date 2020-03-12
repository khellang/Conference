using System.Linq;
using Conference.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conference.SpeakerProfiles
{
    public class SpeakerProfileRepository : EventEntityRepository<SpeakerProfile, SpeakerProfileModel>
    {
        public SpeakerProfileRepository(ConferenceContext context) : base(context)
        {
        }

        protected override IQueryable<SpeakerProfile> Include(IQueryable<SpeakerProfile> source)
        {
            return source.Include(x => x.User);
        }

        protected override IOrderedQueryable<SpeakerProfile> OrderBy(IQueryable<SpeakerProfile> source, string? field)
        {
            return source.OrderBy(x => x.User.GivenName);
        }

        protected override IQueryable<SpeakerProfileModel> Map(IQueryable<SpeakerProfile> query)
        {
            return query.Select(profile => new SpeakerProfileModel
            {
                Id = profile.Id,
                Bio = profile.Bio,
                TagLine = profile.TagLine,
                User = new UserModel
                {
                    Id = profile.User.Id,
                    GivenName = profile.User.GivenName,
                    FamilyName = profile.User.FamilyName,
                }
            });
        }
    }
}
