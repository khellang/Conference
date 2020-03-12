using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Conference.Authorization;
using Conference.Entities;
using Conference.TimeZones;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;

namespace Conference
{
    public class Startup
    {
        private static readonly IDateTimeZoneProvider TimeZoneProvider = DateTimeZoneProviders.Tzdb;

        public Startup(IHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        private IHostEnvironment Environment { get; }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var serviceTypes = new[]
            {
                typeof(ICommandHandler<,>),
                typeof(IQueryHandler<,>),
                typeof(IValidator<>)
            };

            services.Scan(scan => scan
                .FromAssemblyOf<Startup>()
                    .AddClasses(c => c.AssignableToAny(serviceTypes))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()
                    .AddClasses(c => c.AssignableTo(typeof(CrudRepository<,,>)))
                        .AsSelf()
                        .WithScopedLifetime());

            // Misc
            services.AddSingleton<IClock>(SystemClock.Instance);
            services.AddScoped<IQueryExecutor, Mediator>();
            services.AddSingleton<TimeZoneRepository>();
            services.AddScoped<ICommandBus, Mediator>();
            services.Decorate<ICommandBus, ValidationCommandBus>();
            services.AddSingleton(TimeZoneProvider);
            services.AddHttpContextAccessor();
            services.AddDataProtection();

            services.AddProblemDetails(options =>
            {
                options.MapToStatusCode<EntityNotFoundException>(StatusCodes.Status404NotFound);
                options.Map<ValidationException>(ex => new ValidationProblemDetails(ex.Errors));
                options.Map<DbUpdateException>(ex => new DbUpdateProblemDetails(ex));
                options.OnBeforeWriteDetails = (ctx, problem) => problem.AddTraceId(ctx);
            });

            // Authentication / Authorization
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, EventMemberRoleAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, ScopeAuthorizationHandler>();

            services.AddAuthorization(authz =>
            {
                authz.AddPolicy(Constants.Policies.Organizer, p => p.AddRequirements(new EventMemberRoleRequirement(EventMemberRole.Organizer)));
                authz.AddPolicy(Constants.Policies.Reviewer, p => p.AddRequirements(new EventMemberRoleRequirement(EventMemberRole.Reviewer)));
                authz.AddPolicy(Constants.Policies.Admin, p => p.AddRequirements(new EventMemberRoleRequirement(EventMemberRole.Admin)));
            });

            // Entity Framework
            services.AddDbContext<ConferenceContext>(db =>
                db.UseNpgsql(Configuration.GetConnectionString("Default"), npgsql =>
                    npgsql.UseNodaTime().EnableRetryOnFailure()));

            // ASP.NET Core Identity
            services.AddIdentityCore<User>(ConfigureIdentity)
                .AddEntityFrameworkStores<ConferenceContext>()
                .AddDefaultTokenProviders();

            // MVC
            services.AddControllers(controllers =>
            {
                controllers.AddJsonOptions(json =>
                {
                    json.JsonSerializerOptions.ConfigureForNodaTime(TimeZoneProvider);
                    json.JsonSerializerOptions.IgnoreNullValues = true;
                    json.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter(
                            JsonNamingPolicy.CamelCase,
                            allowIntegerValues: false));
                });

                controllers.AddMvcOptions(mvc =>
                {
                    mvc.ReturnHttpNotAcceptable = true;
                });

                controllers.ConfigureApiBehaviorOptions(api =>
                {
                    // Let the ProblemDetails middleware handle error mapping.
                    api.SuppressMapClientErrors = true;
                });
            });
        }

        private static void ConfigureIdentity(IdentityOptions identity)
        {
            identity.ClaimsIdentity.UserNameClaimType = Constants.ClaimTypes.Username;
            identity.ClaimsIdentity.UserIdClaimType = Constants.ClaimTypes.Subject;
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseProblemDetails();

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperIdentity(
                    new Claim(Constants.ClaimTypes.Subject, "7042C1B5-1295-4A0F-B835-B1B0210FE6E6"),
                    new Claim(Constants.ClaimTypes.Scope, Constants.Scopes.ReadEvents, ClaimValueTypes.String, "-- TODO --"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
