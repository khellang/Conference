using System;

namespace Conference.Entities
{
    public class Comment : Entity
    {
        public string Text { get; set; } = null!;

        public bool IsInternal { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; } = null!;

        public Guid SubmissionId { get; set; }

        public Submission Submission { get; set; } = null!;
    }
}