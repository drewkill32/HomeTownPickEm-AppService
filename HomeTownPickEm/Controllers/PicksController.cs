using System.Collections.Generic;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Picks;
using HomeTownPickEm.Application.Picks.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    public class PicksController : ApiControllerBase
    {
        [HttpGet("{leagueId}")]
        public async Task<ActionResult<IEnumerable<PickDto>>> GetPicks(int leagueId)
        {
            var picks = await Mediator.Send(new GetPicks.Query
            {
                LeagueId = leagueId
            });
            return Ok(picks);
        }

        [HttpGet("{leagueId}/{userId}")]
        public async Task<ActionResult<IEnumerable<PickDto>>> GetPicksByUser(int leagueId, string userId)
        {
            var picks = await Mediator.Send(new GetPicks.Query
            {
                LeagueId = leagueId,
                UserId = userId
            });
            return Ok(picks);
        }

        [HttpGet("{leagueId}/{userId}/week/{week}")]
        public async Task<ActionResult<IEnumerable<PickDto>>> GetPicksByUserWeek(int leagueId, string userId, int week)
        {
            var picks = await Mediator.Send(new GetPicks.Query
            {
                LeagueId = leagueId,
                UserId = userId,
                Week = week
            });
            return Ok(picks);
        }

        [HttpGet("{leagueId}/week/{week}")]
        public async Task<ActionResult<IEnumerable<PickDto>>> GetPicksByWeek(int leagueId, int week)
        {
            var picks = await Mediator.Send(new GetPicks.Query
            {
                LeagueId = leagueId,
                Week = week
            });
            return Ok(picks);
        }
    }
}