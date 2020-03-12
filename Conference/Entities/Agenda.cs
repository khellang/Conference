using System.Collections.Generic;
using NodaTime;

namespace Conference.Entities
{
    public class Agenda : EventEntity
    {
        public Agenda()
        {
            TimeSlots = new List<TimeSlot>();
        }

        public LocalDate Date { get; set; }

        public ICollection<TimeSlot> TimeSlots { get; }
    }
}