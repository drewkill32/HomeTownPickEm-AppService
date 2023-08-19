using HomeTownPickEm.Application.Common;
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
            var games = await Mediator.Send(new GetGames.Query
            {
                Season = season
            });
            return Ok(games);
        }

        [HttpGet("{season}/week/{week}")]
        public async Task<ActionResult<GameDto>> GetByWeek(string season, int week)
        {
            var games = await Mediator.Send(new GetGames.Query
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
            var games = await Mediator.Send(new GetGames.Query
            {
                Season = season,
                TeamId = teamId
            });
            return Ok(games);
        }

        [HttpGet("{season}/team/{teamId}/week/{week}")]
        public async Task<ActionResult<GameDto>> GetGameByTeamWeek(string season, int teamId, int week)
        {
            var games = await Mediator.Send(new GetGames.Query
            {
                Season = season,
                TeamId = teamId,
                Week = week
            });
            return Ok(games);
        }

        [HttpPost("load")]
        public ActionResult LoadGames(LoadGames.Command command)
        {
            var logger = HttpContext.RequestServices.GetRequiredService<ILogger<LoadGames>>();
            logger.LogInformation("Enqueueing LoadGames Command");
            var result = Mediator.Enqueue(command.ToString(), command);
            return result ? Accepted() : StatusCode(StatusCodes.Status208AlreadyReported);
        }
        
        [HttpDelete]
        public async Task<ActionResult> DeleteTeams([FromQuery] DeleteGames.Command command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
    }
}