using NodaTime;

namespace Conference.Entities
{
    public class SessionFormat : EventEntity
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public Period Length { get; set; } = null!;
    }
}