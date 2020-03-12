using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Conference
{
    public class DbUpdateProblemDetails : ProblemDetails
    {
        public DbUpdateProblemDetails(DbUpdateException exception)
        {
            if (exception.InnerException is PostgresException pgException)
            {
                if (pgException.SqlState == PostgresErrorCodes.UniqueViolation)
                {
                    Status = StatusCodes.Status409Conflict;
                    Title = "Unique constraint violations.";
                    Detail = pgException.Detail;
                }
            }
        }
    }
}
