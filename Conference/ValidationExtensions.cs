using System;
using FluentValidation;
using NodaTime;

namespace Conference
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string?> MustMatchTimeZone<T>(this IRuleBuilder<T, string?> builder, IDateTimeZoneProvider provider)
        {
            return builder.Must(IsValidTimeZoneId(provider)).WithMessage("'{PropertyName}' must be a valid IANA time zone ID.");
        }

        private static Func<string?, bool> IsValidTimeZoneId(IDateTimeZoneProvider provider)
        {
            return timeZoneId =>
            {
                if (string.IsNullOrEmpty(timeZoneId))
                {
                    return false;
                }

                var zone = provider.GetZoneOrNull(timeZoneId);

                if (zone is null)
                {
                    return false;
                }

                return true;
            };
        }
    }
}
