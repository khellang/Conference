using System;
using System.Collections.Generic;
using NodaTime;

namespace Conference.Entities
{
    public class TimeSlot : Entity
    {
        public TimeSlot()
        {
            Sessions = new List<SessionTimeSlot>();
        }

        public LocalTime StartTime { get; set; }

        public LocalTime EndTime { get; set; }

        public Guid AgendaId { get; set; }

        public Agenda Agenda { get; set; } = null!;

        public ICollection<SessionTimeSlot> Sessions { get; }
    }
}