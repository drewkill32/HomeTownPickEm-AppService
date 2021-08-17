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
        public async Task<ActionResult> AddTeams(AddLeagueTeams.Command command)
        {
            var league = await Mediator.Send(command);
            return CreatedAtAction("GetLeague", league);
        }


        [HttpPost]
        public async Task<ActionResult> CreateLeague(AddLeague.Command command)
        {
            var league = await Mediator.Send(command);

            return CreatedAtAction("GetLeague", league);
        }

        [HttpGet("{name}/{year}", Name = "GetLeague")]
        public async Task<ActionResult<LeagueDto>> GetLeague(string name, string year)
        {
            var league = await Mediator.Send(new GetLeague.Query
            {
                Name = name,
                Year = year
            });

            return Ok(league);
        }
    }
}