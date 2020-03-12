using System.Collections.Generic;
using System.Linq;
using TimeZoneNames;

namespace Conference.TimeZones
{
    public class TimeZoneRepository
    {
        private readonly List<TimeZoneModel> _timeZones = GetTimeZones("en");

        public IReadOnlyCollection<TimeZoneModel> GetAll()
        {
            return _timeZones.AsReadOnly();
        }

        private static List<TimeZoneModel> GetTimeZones(string language)
        {
            return TZNames.GetDisplayNames(language, useIanaZoneIds: true)
                .Select(x => new TimeZoneModel(x.Key, x.Value))
                .ToList();
        }
    }
}
