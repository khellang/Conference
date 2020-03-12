using System;
using Conference.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Conference
{
    public class ConferenceContext : IdentityUserContext<User, Guid>
    {
        public ConferenceContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasPostgresExtension("citext");

            builder.Entity<SubmissionFormat>().HasKey(x => new { x.SubmissionId, x.FormatId });
            builder.Entity<SessionTimeSlot>().HasKey(x => new { x.SessionId, x.TimeSlotId });
            builder.Entity<SessionRoom>().HasKey(x => new { x.SessionId, x.RoomId });

            builder.Entity<EventMember>().HasIndex(x => new { x.EventId, x.UserId }).IsUnique();
            builder.Entity<EventMember>().HasIndex(x => new { x.EventId, x.Email }).IsUnique();
            builder.Entity<EventMember>().Property(x => x.Email).HasColumnType("citext");
            builder.Entity<EventMember>().Property(x => x.Role).HasConversion<string>();

            builder.Entity<Speaker>().HasIndex(x => new { x.SubmissionId, x.Email }).IsUnique();
            builder.Entity<Speaker>().Property(x => x.Email).HasColumnType("citext");

            builder.Entity<Room>().HasIndex(x => new { x.VenueId, x.Name }).IsUnique();
            builder.Entity<Room>().Property(x => x.Name).HasColumnType("citext");

            builder.Entity<Agenda>().HasIndex(x => new { x.EventId, x.Date }).IsUnique();

            builder.Entity<SessionFormat>().HasIndex(x => new { x.EventId, x.Name }).IsUnique();
            builder.Entity<SessionFormat>().Property(x => x.Name).HasColumnType("citext");

            builder.Entity<SpeakerProfile>().HasIndex(x => new { x.UserId, x.EventId }).IsUnique();

            builder.Entity<SpeakerProfile>().HasIndex(x => x.SearchVector).HasMethod("gin");
            builder.Entity<Submission>().HasIndex(x => x.SearchVector).HasMethod("gin");
            builder.Entity<User>().HasIndex(x => x.SearchVector).HasMethod("gin");

            builder.Entity<Comment>().Property(x => x.IsInternal).HasDefaultValue(true);

            builder.UseSnakeCaseNamingConvention();

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Remove ugly ASP.NET Core Identity table name pre- and suffix :)
                entity.SetTableName(entity.GetTableName()
                    .Replace("identity_", string.Empty)
                    .Replace("_guid", string.Empty));
            }
        }
    }
}