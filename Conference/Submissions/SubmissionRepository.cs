using System.Linq;
using Conference.Entities;
using Conference.SessionFormats;
using Microsoft.EntityFrameworkCore;

namespace Conference.Submissions
{
    public class SubmissionRepository : EventEntityRepository<Submission, SubmissionModel>
    {
        public SubmissionRepository(ConferenceContext context) : base(context)
        {
        }

        protected override IQueryable<Submission> Include(IQueryable<Submission> source)
        {
            return source.Include(x => x.Owner).Include(x => x.Formats).ThenInclude(x => x.Format);
        }

        protected override IOrderedQueryable<Submission> OrderBy(IQueryable<Submission> source, string? field)
        {
            return source.OrderBy(x => x.Title);
        }

        protected override IQueryable<SubmissionModel> Map(IQueryable<Submission> query)
        {
            return query.Select(submission => new SubmissionModel
            {
                Id = submission.Id,
                Title = submission.Title,
                Notes = submission.Notes,
                Description = submission.Description,
                Owner = new UserModel
                {
                    Id = submission.Owner.Id,
                    GivenName = submission.Owner.GivenName,
                    FamilyName = submission.Owner.FamilyName,
                },
                Formats = submission.Formats.Select(format => new SessionFormatModel
                {
                    Id = format.Format.Id,
                    Name = format.Format.Name,
                    Description = format.Format.Description,
                    Length = format.Format.Length,
                }),
            });
        }
    }
}
