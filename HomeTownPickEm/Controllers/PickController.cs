using System.Threading.Tasks;
using HomeTownPickEm.Application.Picks;
using HomeTownPickEm.Application.Picks.Commands;
using HomeTownPickEm.Application.Picks.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    public class PickController : ApiControllerBase
    {
        [HttpPut]
        public async Task<ActionResult<PickDto>> CreatePick(SelectTeamCommand command)
        {
            var pick = await Mediator.Send(command);
            return Ok(pick);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PickDto>> GetPick(int id)
        {
            var pick = await Mediator.Send(new GetPickQuery
            {
                Id = id
            });
            return Ok(pick);
        }
    }
}