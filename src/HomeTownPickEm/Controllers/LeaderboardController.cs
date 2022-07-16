using System.Threading.Tasks;
using HomeTownPickEm.Application.Leaderboard;
using HomeTownPickEm.Application.Leaderboard.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    public class LeaderboardController : ApiControllerBase
    {
        [HttpGet("{slug}")]
        public async Task<ActionResult<LeaderBoardDto>> Get(string slug)
        {
            return Ok(await Mediator.Send(new GetLeaderboard.Query
            {
                Slug = slug
            }));
        }
    }
}