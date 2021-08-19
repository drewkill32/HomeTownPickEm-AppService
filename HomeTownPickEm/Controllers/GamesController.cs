using System.Threading.Tasks;
using HomeTownPickEm.Application.Games;
using HomeTownPickEm.Application.Games.Commands;
using HomeTownPickEm.Application.Games.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    //[Authorize]
    public class GamesController : ApiControllerBase
    {
        [HttpGet("all")]
        public async Task<ActionResult<GameDto>> GetAllGames()
        {
            var games = await Mediator.Send(new GetAllGames.Query());
            return Ok(games);
        }

        [HttpGet("week/{week}")]
        public async Task<ActionResult<GameDto>> GetByWeek(int week)
        {
            var games = await Mediator.Send(new GetByTeamWeek.Query
            {
                Week = week
            });
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame([FromRoute] int id)
        {
            var games = await Mediator.Send(new GetGame.Query
            {
                Id = id
            });
            return Ok(games);
        }

        [HttpGet("team/{teamId}/week/{week}")]
        public async Task<ActionResult<GameDto>> GetGame(int teamId, int week)
        {
            var games = await Mediator.Send(new GetByTeamWeek.Query
            {
                TeamId = teamId,
                Week = week
            });
            return Ok(games);
        }

        [HttpPost("load")]
        public async Task<ActionResult<GameDto>> LoadGames(LoadGames.Command command)
        {
            var games = await Mediator.Send(command);
            return Ok(games);
        }
    }
}