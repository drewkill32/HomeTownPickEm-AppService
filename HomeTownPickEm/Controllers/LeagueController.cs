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
        public async Task<ActionResult> CreateLeague(AddLeague.Command command)
        {
            var league = await Mediator.Send(command);

            return CreatedAtAction("GetLeague", league);
        }

        [HttpGet("{name}/{season}", Name = "GetLeague")]
        public async Task<ActionResult<LeagueDto>> GetLeague(string name, string season, [FromQuery] bool picks)
        {
            var league = await Mediator.Send(new GetLeague.Query
            {
                Name = name,
                Year = season,
                IncludePicks = picks
            });

            return Ok(league);
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
    }
}