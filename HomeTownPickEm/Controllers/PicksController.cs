using System.Threading.Tasks;
using HomeTownPickEm.Application.Picks;
using HomeTownPickEm.Application.Picks.Commands;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    public class PicksController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<PickDto>> CreatePick(CreatePick.Command command)
        {
            var pick = await Mediator.Send(command);
            return Ok(pick);
        }
    }
}