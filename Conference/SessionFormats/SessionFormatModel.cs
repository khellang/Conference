using NodaTime;

namespace Conference.SessionFormats
{
    public class SessionFormatModel : EntityModel
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public Period Length { get; set; } = null!;
    }
}