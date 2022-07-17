using HomeTownPickEm.Application.Calendar;
using HomeTownPickEm.Application.Calendar.Commands;
using HomeTownPickEm.Application.Calendar.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    //[Authorize]
    public class CalendarController : ApiControllerBase
    {
        [HttpGet("{week}")]
        public async Task<ActionResult<CalendarDto>> GetCalendar(int week, [FromQuery] GetCalendar.Query query)
        {
            query.Week = week;
            var calendars = await Mediator.Send(query);
            return Ok(calendars);
        }

        [HttpPost("load")]
        public async Task<ActionResult<CalendarDto>> LoadCalendar(LoadCalendar.Command command)
        {
            var calendars = await Mediator.Send(command);
            return Ok(calendars);
        }

        [HttpPut("{calendarId}")]
        public async Task<ActionResult<CalendarDto>> UpdateCalendar(int calendarId, UpdateCutoff.Command command)
        {
            command.CalendarId = calendarId;
            var calendars = await Mediator.Send(command);
            return Ok(calendars);
        }
    }
}