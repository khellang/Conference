using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using FluentValidation.Results;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Http;

namespace Conference
{
    public class ValidationProblemDetails : StatusCodeProblemDetails
    {
        private static readonly JsonNamingPolicy NamingPolicy = JsonNamingPolicy.CamelCase;

        public ValidationProblemDetails(IEnumerable<ValidationFailure> errors) : base(StatusCodes.Status422UnprocessableEntity)
        {
            Title = "Validation failed";
            Errors = new Dictionary<string, string>(errors.Select(e =>
                new KeyValuePair<string, string>(
                    NamingPolicy.ConvertName(e.PropertyName),
                    e.ErrorMessage)));
        }

        public IDictionary<string, string> Errors { get; }
    }
}
