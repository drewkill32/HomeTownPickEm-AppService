using System.Threading.Tasks;
using HomeTownPickEm.Application.Picks;
using HomeTownPickEm.Application.Picks.Commands;
using HomeTownPickEm.Application.Picks.Queries;
using HomeTownPickEm.Application.Picks.Queries.WeeklyPicks;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    public class PicksController : ApiControllerBase
    {
        [HttpGet("{leagueSlug}")]
        public async Task<ActionResult<PicksCollection>> GetPicks([FromRoute] GetPicks.Query query)
        {
            var picks = await Mediator.Send(query);
            return Ok(picks);
        }

        [HttpGet("{leagueSlug}/{userId}")]
        public async Task<ActionResult<PicksCollection>> GetPicksByUser([FromRoute] GetPicks.Query query)
        {
            var picks = await Mediator.Send(query);
            return Ok(picks);
        }

        [HttpGet("{leagueSlug}/{userId}/week/{week}")]
        public async Task<ActionResult<PicksCollection>> GetPicksByUserWeek([FromRoute] GetPicks.Query query)
        {
            var picks = await Mediator.Send(query);
            return Ok(picks);
        }

        [HttpGet("{leagueSlug}/week/{week}")]
        public async Task<ActionResult<PicksCollection>> GetPicksByWeek([FromRoute] GetPicks.Query query)
        {
            var picks = await Mediator.Send(query);
            return Ok(picks);
        }

        [HttpGet("{leagueSlug}/weeklypicks")]
        public async Task<ActionResult<WeeklyPicksDto>> GetWeeklyPicks([FromRoute] GetWeeklyPicks.Query query)
        {
            var picks = await Mediator.Send(query);
            return Ok(picks);
        }

        [HttpGet("{leagueSlug}/weeklypicks/week/{week}")]
        public async Task<ActionResult<WeeklyPicksDto>> GetWeeklyPicksByWeek([FromRoute] GetWeeklyPicks.Query query)
        {
            var picks = await Mediator.Send(query);
            return Ok(picks);
        }

        [HttpPost("{leagueSlug}/updatepickscores")]
        public async Task<ActionResult> UpdatePickScores(string leagueSlug, UpdatePickScores.Command command)
        {
            command.LeagueSlug = leagueSlug;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}