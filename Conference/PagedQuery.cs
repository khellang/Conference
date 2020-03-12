namespace Conference
{
    public class PagedQuery
    {
        public int? Page { get; set; }

        public int? Size { get; set; }

        public string? OrderBy { get; set; }
    }
}