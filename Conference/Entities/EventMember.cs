using System;

namespace Conference.Entities
{
    public class EventMember : EventEntity
    {
        public string Email { get; set; } = null!;

        public EventMemberRole Role { get; set; }

        public Guid? UserId { get; set; }

        public User? User { get; set; }
    }
}