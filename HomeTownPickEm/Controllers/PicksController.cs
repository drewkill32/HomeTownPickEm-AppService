using System.Threading.Tasks;
using HomeTownPickEm.Application.Picks;
using HomeTownPickEm.Application.Picks.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    public class PicksController : ApiControllerBase
    {
        [HttpGet("{leagueSlug}")]
        public async Task<ActionResult<PicksCollection>> GetPicks(string leagueSlug)
        {
            var picks = await Mediator.Send(new GetPicks.Query
            {
                LeagueSlug = leagueSlug
            });
            return Ok(picks);
        }

        [HttpGet("{leagueSlug}/{userId}")]
        public async Task<ActionResult<PicksCollection>> GetPicksByUser(string leagueSlug, string userId)
        {
            var picks = await Mediator.Send(new GetPicks.Query
            {
                LeagueSlug = leagueSlug,
                UserId = userId
            });
            return Ok(picks);
        }

        [HttpGet("{leagueSlug}/{userId}/week/{week}")]
        public async Task<ActionResult<PicksCollection>> GetPicksByUserWeek(string leagueSlug, string userId, int week)
        {
            var picks = await Mediator.Send(new GetPicks.Query
            {
                LeagueSlug = leagueSlug,
                UserId = userId,
                Week = week
            });
            return Ok(picks);
        }

        [HttpGet("{leagueSlug}/week/{week}")]
        public async Task<ActionResult<PicksCollection>> GetPicksByWeek(string leagueSlug, int week)
        {
            var picks = await Mediator.Send(new GetPicks.Query
            {
                LeagueSlug = leagueSlug,
                Week = week
            });
            return Ok(picks);
        }
    }
}