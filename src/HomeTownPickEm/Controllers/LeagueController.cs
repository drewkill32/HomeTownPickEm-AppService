using HomeTownPickEm.Application.Leaderboard;
using HomeTownPickEm.Application.Leaderboard.Queries;
using HomeTownPickEm.Application.Leagues;
using HomeTownPickEm.Application.Leagues.Commands;
using HomeTownPickEm.Application.Leagues.Queries;
using HomeTownPickEm.Application.Teams;
using HomeTownPickEm.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    public class LeagueController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> CreateLeague(AddLeague.Command command)
        {
            var league = await Mediator.Send(command);

            return CreatedAtAction("GetLeague", league);
        }

        [HttpGet("{LeagueSlug}/{Season}/leaderboard")]
        public async Task<ActionResult<LeaderBoardDto>> Get([FromRoute] GetLeaderboard.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{slug}/{season}", Name = "GetLeague")]
        public async Task<ActionResult<LeagueDto>> GetLeague(string slug, string season, [FromQuery] bool picks)
        {
            var league = await Mediator.Send(new GetLeague.Query
            {
                LeagueSlug = slug,
                Year = season,
                IncludePicks = picks
            });

            return Ok(league);
        }

        [HttpGet("{LeagueSlug}/{Season}/game/{gameId}")]
        public async Task<ActionResult<IEnumerable<UserPickResponse>>> GetMemberGamePicks(
            [FromRoute] GetMemberPicksByGame.Query query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}/availableteams")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetNotInLeague(int id)
        {
            var query = new TeamsNotInLeague.Query
            {
                LeagueId = id
            };
            var teams = await Mediator.Send(query);
            return Ok(teams);
        }

        [HttpPost("{leagueId}/updateleaguepicks")]
        public async Task<ActionResult> UpdateLeague([FromRoute] UpdateLeague.Command command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLeagueDto>>> GetLeagues()
        {
            var leagues = await Mediator.Send(new GetLeagues.Query());
            return Ok(leagues);
        }
    }
}