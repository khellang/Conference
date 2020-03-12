using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Conference
{
    public static class ProblemDetailsExtensions
    {
        public static void AddTraceId(this ProblemDetails problem, HttpContext context)
        {
            var traceId = Activity.Current?.Id ?? context?.TraceIdentifier;
            if (traceId != null)
            {
                problem.Extensions["traceId"] = traceId;
            }
        }
    }
}
