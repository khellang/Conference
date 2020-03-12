using System;

namespace Conference.Entities
{
    public class SubmissionFormat
    {
        public Guid SubmissionId { get; set; }

        public Submission Submission { get; set; } = null!;

        public Guid FormatId { get; set; }

        public SessionFormat Format { get; set; } = null!;
    }
}