using System.Collections.Generic;
using NodaTime;

namespace Conference.Entities
{
    public class Event : Entity
    {
        public Event()
        {
            Venues = new List<Venue>();
            SpeakerProfiles = new List<SpeakerProfile>();
            Agendas = new List<Agenda>();
            Submissions = new List<Submission>();
            Members = new List<EventMember>();
            SessionFormats = new List<SessionFormat>();
        }

        public string Name { get; set; } = null!;

        public LocalDateTime StartTime { get; set; }

        public LocalDateTime EndTime { get; set; }

        public LocalDateTime CfpOpenTime { get; set; }

        public LocalDateTime CfpCloseTime { get; set; }

        public string TimeZoneId { get; set; } = null!;

        public string TimeZoneRules { get; set; } = null!;

        public ICollection<EventMember> Members { get; }

        public ICollection<Venue> Venues { get; }

        public ICollection<SpeakerProfile> SpeakerProfiles { get; }

        public ICollection<Agenda> Agendas { get; }

        public ICollection<Submission> Submissions { get; }

        public ICollection<SessionFormat> SessionFormats { get; }
    }
}