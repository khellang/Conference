using System;
using System.Threading;
using System.Threading.Tasks;
using Conference.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NodaTime;

namespace Conference.Events
{
    public static class CreateEvent
    {
        public class Command : ICommand<EventModel>
        {
            public Guid? Id { get; set; }

            public string? Name { get; set; }

            public LocalDateTime? StartTime { get; set; }

            public LocalDateTime? EndTime { get; set; }

            public LocalDateTime? CfpOpenTime { get; set; }

            public LocalDateTime? CfpCloseTime { get; set; }

            public string? TimeZoneId { get; set; }
        }

        public class Handler : ICommandHandler<Command, EventModel>
        {
            public Handler(EventRepository repository, UserManager<User> userManager, IDateTimeZoneProvider provider, IHttpContextAccessor accessor)
            {
                Repository = repository;
                UserManager = userManager;
                Provider = provider;
                Accessor = accessor;
            }

            private EventRepository Repository { get; }

            private UserManager<User> UserManager { get; }

            private IDateTimeZoneProvider Provider { get; }

            private IHttpContextAccessor Accessor { get; }

            public async ValueTask<EventModel> Handle(Command command, CancellationToken cancellationToken)
            {
                var @event = new Event
                {
                    Id = command.Id ?? Guid.NewGuid(),
                    Name = command.Name!,
                    StartTime = command.StartTime!.Value,
                    EndTime = command.EndTime!.Value,
                    CfpOpenTime = command.CfpOpenTime!.Value,
                    CfpCloseTime = command.CfpCloseTime!.Value,
                    TimeZoneId = command.TimeZoneId!,
                    TimeZoneRules = Provider.VersionId,
                };

                var user = await UserManager.GetUserAsync(Accessor.HttpContext.User);

                @event.Members.Add(new EventMember { Email = user.Email, Role = EventMemberRole.Admin, User = user });

                var days = Period.Between(@event.StartTime.Date, @event.EndTime.Date.PlusDays(1)).Days;

                for (var i = 0; i < days; i++)
                {
                    @event.Agendas.Add(new Agenda
                    {
                        Id = Guid.NewGuid(),
                        Date = @event.StartTime.Date.PlusDays(i),
                    });
                }

                return await Repository.Create(@event, cancellationToken);
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator(IDateTimeZoneProvider provider)
            {
                if (provider is null)
                {
                    throw new ArgumentNullException(nameof(provider));
                }

                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.EndTime).NotNull();
                RuleFor(x => x.StartTime).NotNull();
                RuleFor(x => x.CfpOpenTime).NotNull();
                RuleFor(x => x.CfpCloseTime).NotNull();

                RuleFor(x => x.EndTime)
                    .GreaterThan(x => x.StartTime)
                    .When(x => x.StartTime.HasValue);

                RuleFor(x => x.CfpCloseTime)
                    .GreaterThan(x => x.CfpOpenTime)
                    .When(x => x.CfpOpenTime.HasValue);

                RuleFor(x => x.TimeZoneId).MustMatchTimeZone(provider);
            }
        }
    }
}
