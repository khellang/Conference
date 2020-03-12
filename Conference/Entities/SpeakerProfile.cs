using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace Conference.Entities
{
    public class SpeakerProfile : EventEntity
    {
        public SpeakerProfile()
        {
            Speakers = new List<Speaker>();
        }

        public string TagLine { get; set; } = null!;

        public string Bio { get; set; } = null!;

        public NpgsqlTsVector SearchVector { get; set; } = null!;

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public ICollection<Speaker> Speakers { get; }
    }
}