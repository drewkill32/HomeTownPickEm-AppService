using System.Collections.Generic;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Leagues;
using HomeTownPickEm.Application.Leagues.Commands;
using HomeTownPickEm.Application.Leagues.Queries;
using HomeTownPickEm.Application.Teams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    public class LeagueController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> CreateLeague(AddLeagueCommand command)
        {
            var league = await Mediator.Send(command);

            return CreatedAtAction("GetLeague", league);
        }

        [HttpGet("{name}/{season}", Name = "GetLeague")]
        public async Task<ActionResult<LeagueDto>> GetLeague(string name, string season, [FromQuery] bool picks)
        {
            var league = await Mediator.Send(new GetLeagueQuery
            {
                Name = name,
                Year = season,
                IncludePicks = picks
            });

            return Ok(league);
        }

        [HttpGet("{leagueSlug}/game/{gameId}")]
        public async Task<ActionResult<IEnumerable<UserPickResponse>>> GetMemberGamePicks(
            [FromRoute] GetMemberPicksByGameQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}/availableteams")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetNotInLeague(int id)
        {
            var query = new TeamsNotInLeagueQuery
            {
                LeagueId = id
            };
            var teams = await Mediator.Send(query);
            return Ok(teams);
        }

        [HttpPost("{leagueId}/updateleaguepicks")]
        public async Task<ActionResult> UpdateLeague([FromRoute] UpdateLeagueCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}