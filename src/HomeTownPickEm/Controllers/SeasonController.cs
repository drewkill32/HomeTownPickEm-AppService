using HomeTownPickEm.Application.Seasons.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers;

public class SeasonController : ApiControllerBase
{
    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentSeason()
    {
        var result = await Mediator.Send(new CurrentSeason.Query());
        return Ok(result);
    }
}