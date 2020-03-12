using System;
using NodaTime;

namespace Conference
{
    public static class LocalDateTimeExtensions
    {
        public static Instant? ToInstant(this LocalDateTime? local, IDateTimeZoneProvider provider, string timeZoneId)
        {
            return local?.ToInstant(provider, timeZoneId);
        }

        public static Instant ToInstant(this LocalDateTime local, IDateTimeZoneProvider provider, string timeZoneId)
        {
            var timeZone = provider.GetZoneOrNull(timeZoneId);

            if (timeZone is null)
            {
                throw new ArgumentException(nameof(timeZoneId));
            }

            return local.InZoneLeniently(timeZone).ToInstant();
        }
    }
}