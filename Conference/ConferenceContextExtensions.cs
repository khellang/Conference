using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conference.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using NodaTime.TimeZones;

namespace Conference
{
    public static class ConferenceContextExtensions
    {
        private static readonly SessionFormat Keynote = new SessionFormat { Name = "Keynote", Length = Period.FromMinutes(60) };

        private static readonly SessionFormat Regular = new SessionFormat { Name = "Regular", Length = Period.FromMinutes(60) };

        public static async Task SeedAsync(this ConferenceContext context, UserManager<User> userStore, CancellationToken cancellationToken = default)
        {
            if (await context.Events.AnyAsync(cancellationToken))
            {
                return;
            }

            // TODO: Use HasGeneratedTsVectorProperty instead of the following when Npgsql 5.0 is released.
            // See https://github.com/npgsql/efcore.pg/issues/1253 for more details.

            await context.Database.ExecuteSqlRawAsync(
                @"CREATE TRIGGER submission_search_vector_update BEFORE INSERT OR UPDATE
                  ON submission FOR EACH ROW EXECUTE PROCEDURE
                  tsvector_update_trigger(search_vector, 'pg_catalog.english', title, description);", cancellationToken);

            await context.Database.ExecuteSqlRawAsync(
                @"CREATE TRIGGER submission_search_vector_update BEFORE INSERT OR UPDATE
                  ON speaker_profile FOR EACH ROW EXECUTE PROCEDURE
                  tsvector_update_trigger(search_vector, 'pg_catalog.english', tag_line, bio);", cancellationToken);

            await context.Database.ExecuteSqlRawAsync(
                @"CREATE TRIGGER submission_search_vector_update BEFORE INSERT OR UPDATE
                  ON ""user"" FOR EACH ROW EXECUTE PROCEDURE
                  tsvector_update_trigger(search_vector, 'pg_catalog.english', email, user_name, given_name, family_name);", cancellationToken);

            var user = new User
            {
                Id = new Guid("7042C1B5-1295-4A0F-B835-B1B0210FE6E6"),
                UserName = "khellang",
                GivenName = "Kristian",
                FamilyName = "Hellang",
                Email = "kristian@hellang.com",
            };

            var result = await userStore.CreateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create user:\n{string.Join('\n', result.Errors.Select(x => x.Description))}");
            }

            var ndc = new Event
            {
                Id = new Guid("f225d953-bf14-449f-8de5-df48500b4ff9"),
                Name = "NDC Oslo 2020",
                StartTime = new LocalDateTime(2020, 06, 10, 09, 00),
                EndTime = new LocalDateTime(2020, 06, 12, 17, 00),
                CfpOpenTime = new LocalDateTime(2019, 11, 14, 12, 00),
                CfpCloseTime = new LocalDateTime(2020, 02, 16, 00, 00),
                TimeZoneId = TzdbDateTimeZoneSource.Default.GetSystemDefaultId(),
                TimeZoneRules = DateTimeZoneProviders.Tzdb.VersionId,
                Venues =
                {
                    new Venue
                    {
                        Name = "Oslo Spektrum",
                        Location = "Sonja Henies plass 2, 0185 Oslo",
                        Rooms =
                        {
                            new Room { Name = "Room 1", Capacity = 250 },
                            new Room { Name = "Room 2", Capacity = 250 },
                            new Room { Name = "Room 3", Capacity = 250 },
                            new Room { Name = "Room 4", Capacity = 250 },
                            new Room { Name = "Room 5", Capacity = 450 },
                            new Room { Name = "Room 6", Capacity = 400 },
                            new Room { Name = "Room 7", Capacity = 450 },
                            new Room { Name = "Room 8", Capacity = 300 },
                            new Room { Name = "Room 9", Capacity = 150 },
                        }
                    },
                },
                SessionFormats =
                {
                    Keynote,
                    Regular,
                    new SessionFormat { Name = "Lightning Talk", Length = Period.FromMinutes(10) },
                    new SessionFormat { Name = "Workshop", Length = Period.FromMinutes(120) },
                },
                Members =
                {
                    new EventMember { Email = user.Email, Role = EventMemberRole.Admin, User = user },
                },
            };

            var speakerProfile = new SpeakerProfile
            {
                User = user,
                TagLine = "The most awesome speaker, ever!",
                Bio = "This is the bio. Ideally it should be longer, but I can't think of something to write.",
            };

            ndc.SpeakerProfiles.Add(speakerProfile);

            ndc.Submissions.Add(new Submission
            {
                Owner = user,
                Title = "This is my first talk, hope you like it.",
                Description = "This is the talk description. Short and simple.",
                Formats = { new SubmissionFormat { Format = Regular} },
                Speakers = { new Speaker { Email = user.Email, Profile = speakerProfile } }
            });

            GenerateAgenda(ndc);

            await context.AddAsync(ndc, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        private static void GenerateAgenda(Event @event)
        {
            var days = Period.Between(@event.StartTime.Date, @event.EndTime.Date.PlusDays(1)).Days;

            for (var day = 0; day < days; day++)
            {
                var isLongDay = day == 0 || day == 1;
                var hasKeynote = day == 0 || day == 2;

                var agenda = new Agenda
                {
                    Date = @event.StartTime.Date.PlusDays(day),
                };

                var startTime = @event.StartTime.TimeOfDay;

                var sessionCount = isLongDay ? 7 : 6;

                for (var i = 0; i < sessionCount; i++)
                {
                    var timeSlot = new TimeSlot
                    {
                        StartTime = startTime,
                        EndTime = startTime = startTime.PlusMinutes(60),
                    };

                    startTime = startTime.PlusMinutes(20); // 20 minute break

                    if (i == 2)
                    {
                        startTime = startTime.PlusMinutes(40); // 20 minute extra lunch
                    }

                    if (hasKeynote && i == 0)
                    {
                        timeSlot.Sessions.Add(new SessionTimeSlot
                        {
                            Session = new Session
                            {
                                Format = Keynote,
                                Title = "Keynote",
                            },
                        });
                    }
                    else
                    {
                        foreach (var room in @event.Venues.SelectMany(x => x.Rooms))
                        {
                            timeSlot.Sessions.Add(new SessionTimeSlot
                            {
                                Session = new Session
                                {
                                    Format = Regular,
                                    Rooms =
                                    {
                                        new SessionRoom
                                        {
                                            Room = room
                                        }
                                    }
                                },
                            });
                        }
                    }

                    agenda.TimeSlots.Add(timeSlot);
                }

                @event.Agendas.Add(agenda);
            }
        }
    }
}