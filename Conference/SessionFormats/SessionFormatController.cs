using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Conference.SessionFormats
{
    [ApiController]
    [Route("events/{eventId:guid}/session-formats")]
    public class SessionFormatController : ControllerBase
    {
        public SessionFormatController(IQueryExecutor mediator)
        {
            Mediator = mediator;
        }

        private IQueryExecutor Mediator { get; }

        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PageModel<SessionFormatModel>>> GetAll([FromRoute] Guid eventId, CancellationToken cancellationToken)
        {
            var query = new GetSessionFormatsByEventId(eventId);

            var result = await Mediator.Execute<GetSessionFormatsByEventId, PageModel<SessionFormatModel>>(query, cancellationToken);

            return Ok(result);
        }
    }
}
