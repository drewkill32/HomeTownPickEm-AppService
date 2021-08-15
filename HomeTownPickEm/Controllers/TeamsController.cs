using System.Threading.Tasks;
using HomeTownPickEm.Teams.Commands;
using HomeTownPickEm.Teams.Commands.LoadTeams;
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