namespace Conference.TimeZones
{
    public class TimeZoneModel
    {
        public TimeZoneModel(string value, string name)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }

    }
}
