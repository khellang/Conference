using System;
using System.Collections.Generic;

namespace Conference.Entities
{
    public class Session : Entity
    {
        public Session()
        {
            Rooms = new List<SessionRoom>();
            Submissions = new List<Submission>();
            TimeSlots = new List<SessionTimeSlot>();
        }

        public string? Title { get; set; }

        public Guid FormatId { get; set; }

        public SessionFormat Format { get; set; } = null!;

        public ICollection<SessionRoom> Rooms { get; }

        public ICollection<Submission> Submissions { get; }

        public ICollection<SessionTimeSlot> TimeSlots { get; }
    }

    public class SessionRoom
    {
        public Guid SessionId { get; set; }

        public Session Session { get; set; } = null!;

        public Guid RoomId { get; set; }

        public Room Room { get; set; } = null!;
    }
}