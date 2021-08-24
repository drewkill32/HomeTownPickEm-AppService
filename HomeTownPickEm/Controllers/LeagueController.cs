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


        [HttpPut("{name}/{season}/user/{userId}")]
        public async Task<ActionResult> UpdateLeague(string name, string season, string userId,
            UpdateLeague.Command command)
        {
            command.Name = name;
            command.Season = season;
            command.MemberIds = new[] { userId };
            command.KeepExisting = true;
            var league = await Mediator.Send(command);
            return CreatedAtAction("GetLeague", league);
        }

        [HttpPut("{name}/{season}/team/{teamId}")]
        public async Task<ActionResult> UpdateLeague(string name, string season, int teamId,
            UpdateLeague.Command command)
        {
            command.Name = name;
            command.Season = season;
            command.TeamIds = new[] { teamId };
            command.KeepExisting = true;
            var league = await Mediator.Send(command);
            return CreatedAtAction("GetLeague", league);
        }
    }
}