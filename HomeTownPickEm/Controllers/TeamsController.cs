using System.Collections.Generic;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Teams;
using HomeTownPickEm.Application.Teams.Commands.LoadTeams;
using HomeTownPickEm.Application.Teams.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    public class TeamsController : ApiControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TeamDto>>> Get([FromQuery] string conference,
            [FromQuery] string name, [FromQuery] int? top)
        {
            var teams = await Mediator.Send(new GetTeams.Query
            {
                Conference = conference,
                Name = name,
                Top = top
            });
            return Ok(teams);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetAll()
        {
            var teams = await Mediator.Send(new GetTeams.Query
            {
                IncludeNoConference = true
            });
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