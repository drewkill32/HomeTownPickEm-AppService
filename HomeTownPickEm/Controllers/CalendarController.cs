using System.Threading.Tasks;
using HomeTownPickEm.Application.Calendar;
using HomeTownPickEm.Application.Calendar.Commands;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    //[Authorize]
    public class CalendarController : ApiControllerBase
    {
        [HttpPost("load")]
        public async Task<ActionResult<CalendarDto>> LoadCalendar(LoadCalendar.Command command)
        {
            var calendars = await Mediator.Send(command);
            return Ok(calendars);
        }
    }
}