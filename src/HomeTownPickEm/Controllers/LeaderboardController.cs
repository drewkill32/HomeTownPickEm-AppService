using HomeTownPickEm.Application.Leaderboard.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers;

public class LeaderboardController : ApiControllerBase
{
    [HttpGet("weekly/season/{SeasonId}/week/{Week}")]
    public async Task<ActionResult> GetWeeklyLeaderboard([FromRoute] GetWeeklyLeaderboard.Query query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
}