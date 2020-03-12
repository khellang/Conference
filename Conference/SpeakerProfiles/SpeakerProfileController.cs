using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Conference.SpeakerProfiles
{
    [ApiController]
    [Route("events/{eventId:guid}/speakers")]
    public class SpeakerProfileController : ControllerBase
    {
        public SpeakerProfileController(IQueryExecutor mediator)
        {
            Mediator = mediator;
        }

        private IQueryExecutor Mediator { get; }

        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PageModel<SpeakerProfileModel>>> GetAll([FromRoute] Guid eventId, CancellationToken cancellationToken)
        {
            var query = new GetSpeakerProfilesByEventId(eventId);

            var result = await Mediator.Execute<GetSpeakerProfilesByEventId, PageModel<SpeakerProfileModel>>(query, cancellationToken);

            return Ok(result);
        }
    }
}
