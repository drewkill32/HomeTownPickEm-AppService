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
        [HttpGet("{season}/all")]
        public async Task<ActionResult<GameDto>> GetAllGames(string season)
        {
            var games = await Mediator.Send(new GetAllGames.Query
            {
                Season = season
            });
            return Ok(games);
        }

        [HttpGet("{season}/week/{week}")]
        public async Task<ActionResult<GameDto>> GetByWeek(string season, int week)
        {
            var games = await Mediator.Send(new GetByTeamWeek.Query
            {
                Season = season,
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

        [HttpGet("{season}/team/{teamId}/")]
        public async Task<ActionResult<GameDto>> GetGameByTeam(string season, int teamId)
        {
            var games = await Mediator.Send(new GetByTeamWeek.Query
            {
                Season = season,
                TeamId = teamId
            });
            return Ok(games);
        }

        [HttpGet("{season}/team/{teamId}/week/{week}")]
        public async Task<ActionResult<GameDto>> GetGameByTeamWeek(string season, int teamId, int week)
        {
            var games = await Mediator.Send(new GetByTeamWeek.Query
            {
                Season = season,
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