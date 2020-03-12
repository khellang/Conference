using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Conference.Submissions
{
    [ApiController]
    [Route("events/{eventId:guid}/submissions")]
    public class SubmissionController : ControllerBase
    {
        public SubmissionController(IQueryExecutor mediator)
        {
            Mediator = mediator;
        }

        private IQueryExecutor Mediator { get; }

        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PageModel<SubmissionModel>>> GetAll([FromRoute] Guid eventId, CancellationToken cancellationToken)
        {
            var query = new GetSubmissionsByEventId(eventId);

            var result = await Mediator.Execute<GetSubmissionsByEventId, PageModel<SubmissionModel>>(query, cancellationToken);

            return Ok(result);
        }
    }
}
