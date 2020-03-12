using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeZoneNames;

namespace Conference.TimeZones
{
    [ApiController]
    [Route("time-zones")]
    public class TimeZoneController : ControllerBase
    {
        public TimeZoneController(TimeZoneRepository repository)
        {
            Repository = repository;
        }

        private TimeZoneRepository Repository { get; }

        [HttpGet]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<List<TimeZoneModel>> GetAll()
        {
            return Ok(Repository.GetAll());
        }
    }
}
