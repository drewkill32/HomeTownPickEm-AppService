using System.Collections.Generic;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Teams.Commands;
using HomeTownPickEm.Application.Teams.Commands.LoadTeams;
using HomeTownPickEm.Application.Teams.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    //[Authorize]
    public class TeamsController : ApiControllerBase
    {
        [HttpPost("load")]
        public async Task<ActionResult<IEnumerable<TeamDto>>> LoadTeams()
        {
            var teams = await Mediator.Send(new LoadTeams.Command());
            return Ok(teams);
        }

        [HttpGet("all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetAll()
        {
            var teams = await Mediator.Send(new GetAll.Query());
            return Ok(teams);
        }
    }
}