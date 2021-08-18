using System.Threading.Tasks;
using HomeTownPickEm.Application.Leagues;
using HomeTownPickEm.Application.Leagues.Commands;
using HomeTownPickEm.Application.Leagues.Queries;
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
        public async Task<ActionResult<LeagueDto>> GetLeague(string name, string season)
        {
            var league = await Mediator.Send(new GetLeague.Query
            {
                Name = name,
                Year = season
            });

            return Ok(league);
        }

        [HttpPut("{name}/{season}")]
        public async Task<ActionResult> UpdateLeague(string name, string season, UpdateLeague.Command command)
        {
            command.Name = name;
            command.Season = season;
            var league = await Mediator.Send(command);
            return CreatedAtAction("GetLeague", league);
        }
    }
}