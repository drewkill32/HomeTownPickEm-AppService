using System.Security.Claims;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Picks;
using HomeTownPickEm.Application.Picks.Commands;
using HomeTownPickEm.Application.Picks.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    public class PickController : ApiControllerBase
    {
        [HttpPut("{id}")]
        public async Task<ActionResult<PickDto>> CreatePick(int id, SelectTeam.Command command)
        {
            command.Id = id;

            command.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var pick = await Mediator.Send(command);
            return Ok(pick);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PickDto>> GetPick(int id)
        {
            var pick = await Mediator.Send(new GetPick.Query
            {
                Id = id
            });
            return Ok(pick);
        }
    }
}