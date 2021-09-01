using System.Threading.Tasks;
using HomeTownPickEm.Application.Calendar;
using HomeTownPickEm.Application.Calendar.Commands;
using HomeTownPickEm.Application.Calendar.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    //[Authorize]
    public class CalendarController : ApiControllerBase
    {
        [HttpGet("{leagueSlug}")]
        public async Task<ActionResult<CalendarDto>> GetCalendar(string leagueSlug, [FromQuery] GetCalendar.Query query)
        {
            query.LeagueSlug = leagueSlug;
            var calendars = await Mediator.Send(query);
            return Ok(calendars);
        }

        [HttpPost("load")]
        public async Task<ActionResult<CalendarDto>> LoadCalendar(LoadCalendar.Command command)
        {
            var calendars = await Mediator.Send(command);
            return Ok(calendars);
        }
    }
}