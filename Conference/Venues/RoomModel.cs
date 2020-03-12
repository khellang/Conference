namespace Conference.Venues
{
    public class RoomModel : EntityModel
    {
        public string Name { get; set; } = null!;

        public int? Capacity { get; set; }
    }
}
