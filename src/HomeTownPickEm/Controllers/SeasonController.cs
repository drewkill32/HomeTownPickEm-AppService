using HomeTownPickEm.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers;

public class SeasonController : ApiControllerBase
{
    [HttpGet("current")]
    public IActionResult GetCurrentSeason()
    {
        var date = HttpContext.RequestServices.GetRequiredService<ISystemDate>();
        return Ok(new { Season = date.Now.Year.ToString() });
    }
}