using System.Collections.Generic;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Teams;
using HomeTownPickEm.Application.Teams.Commands.LoadTeams;
using HomeTownPickEm.Application.Teams.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    //[Authorize]
    public class TeamsController : ApiControllerBase
    {
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetAll()
        {
            var teams = await Mediator.Send(new GetAll.Query());
            return Ok(teams);
        }

        [HttpPost("load")]
        public async Task<ActionResult<IEnumerable<TeamDto>>> LoadTeams()
        {
            var teams = await Mediator.Send(new LoadTeams.Command());
            return Ok(teams);
        }
    }
}