using System.Threading.Tasks;
using HomeTownPickEm.Application.Teams.Commands;
using HomeTownPickEm.Application.Teams.Commands.LoadTeams;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    //[Authorize]
    public class TeamsController : ApiControllerBase
    {
        [HttpPost("load")]
        public async Task<ActionResult<TeamDto>> LoadTeams()
        {
            var teams = await Mediator.Send(new LoadTeams.Command());
            return Ok(teams);
        }
    }
}