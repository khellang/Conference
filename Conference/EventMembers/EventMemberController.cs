using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Conference.EventMembers
{
    [ApiController]
    [Route("events/{eventId:guid}/members")]
    public class EventMemberController : ControllerBase
    {
        public EventMemberController(IQueryExecutor mediator)
        {
            Mediator = mediator;
        }

        private IQueryExecutor Mediator { get; }

        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PageModel<EventMemberModel>>> GetAll([FromRoute] Guid eventId, CancellationToken cancellationToken)
        {
            var query = new GetEventMembersByEventId(eventId);

            var result = await Mediator.Execute<GetEventMembersByEventId, PageModel<EventMemberModel>>(query, cancellationToken);

            return Ok(result);
        }
    }
}
