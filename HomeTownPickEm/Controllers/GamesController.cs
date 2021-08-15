using System.Threading.Tasks;
using HomeTownPickEm.Application.Games;
using HomeTownPickEm.Application.Games.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    //[Authorize]
    public class GamesController : ApiControllerBase
    {
        [HttpPost("load")]
        public async Task<ActionResult<GameDto>> LoadCalendar(LoadGames.Command command)
        {
            var games = await Mediator.Send(command);
            return Ok(games);
        }
    }
}