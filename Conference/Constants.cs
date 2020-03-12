namespace Conference
{

    public static class Constants
    {
        public static class Scopes
        {
            public static readonly string ReadEvents = "events:read";
        }

        public static class Policies
        {
            public static readonly string Organizer = nameof(Organizer);

            public static readonly string Reviewer = nameof(Reviewer);

            public static readonly string Admin = nameof(Admin);
        }

        public static class ClaimTypes
        {
            public static readonly string Subject = "sub";

            public static readonly string Scope = "scope";

            public static readonly string Username = "username";
        }
    }
}
