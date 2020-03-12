using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Conference.Venues
{
    [ApiController]
    [Route("events/{eventId:guid}/venues")]
    public class VenueController : ControllerBase
    {
        public VenueController(IQueryExecutor mediator)
        {
            Mediator = mediator;
        }

        private IQueryExecutor Mediator { get; }

        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PageModel<VenueModel>>> GetAll([FromRoute] Guid eventId, CancellationToken cancellationToken)
        {
            var query = new GetVenuesByEventId(eventId);

            var result = await Mediator.Execute<GetVenuesByEventId, PageModel<VenueModel>>(query, cancellationToken);

            return Ok(result);
        }
    }
}
