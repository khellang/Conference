using Conference.Entities;

namespace Conference.EventMembers
{
    public class EventMemberModel : EntityModel
    {
        public string Email { get; set; } = null!;

        public EventMemberRole Role { get; set; }

        public UserModel? User { get; set; }
    }
}