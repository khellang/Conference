using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Conference.Events
{
    [ApiController]
    [Route("events")]
    public class EventController : ControllerBase
    {
        public EventController(IQueryExecutor query, ICommandBus command)
        {
            Query = query;
            Command = command;
        }

        private IQueryExecutor Query { get; }

        private ICommandBus Command { get; }

        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PageModel<EventModel>>> Get([FromRoute] GetEvents query, CancellationToken cancellationToken)
        {
            return Ok(await Query.Execute<GetEvents, PageModel<EventModel>>(query, cancellationToken));
        }

        [HttpGet("{id:guid}")]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EventModel>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var query = new GetEventById(id);

            var @event = await Query.Execute<GetEventById, EventModel>(query, cancellationToken);

            if (@event is null)
            {
                return NotFound();
            }

            return Ok(@event);
        }

        [HttpPost]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<EventModel>> Create([FromBody] CreateEvent.Command command, CancellationToken cancellationToken)
        {
            var @event = await Command.Handle<CreateEvent.Command, EventModel>(command, cancellationToken);

            var url = Url.Action(nameof(GetById), new { id = @event.Id });

            return Created(url, @event);
        }
    }
}
