using System.Collections.Generic;
using Conference.SessionFormats;

namespace Conference.Submissions
{
    public class SubmissionModel : EntityModel
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? Notes { get; set; }

        public UserModel Owner { get; set; } = null!;

        public IEnumerable<SessionFormatModel> Formats { get; set; } = null!;
    }
}