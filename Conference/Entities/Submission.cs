using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace Conference.Entities
{
    public class Submission : EventEntity
    {
        public Submission()
        {
            Speakers = new List<Speaker>();
            Comments = new List<Comment>();
            Formats = new List<SubmissionFormat>();
        }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? Notes { get; set; }

        public NpgsqlTsVector SearchVector { get; set; } = null!;

        public Guid OwnerId { get; set; }

        public User Owner { get; set; } = null!;

        public Guid? SessionId { get; set; }

        public Session? Session { get; set; } = null!;

        public ICollection<Speaker> Speakers { get; }

        public ICollection<Comment> Comments { get; }

        public ICollection<SubmissionFormat> Formats { get; }
    }
}