using System;

namespace Conference.Entities
{
    public class Speaker : Entity
    {
        public string Email { get; set; } = null!;

        public Guid SubmissionId { get; set; }

        public Submission Submission { get; set; } = null!;

        public Guid? ProfileId { get; set; }

        public SpeakerProfile? Profile { get; set; }
    }
}