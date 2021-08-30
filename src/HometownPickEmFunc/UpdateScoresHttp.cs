using System.Threading.Tasks;
using HometownPickEmFunc.CFBD;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace HometownPickEmFunc
{
    public class UpdateScoresHttp
    {
        private readonly IUpdateGamesService _updateGamesService;

        public UpdateScoresHttp(IUpdateGamesService updateGamesService)
        {
            _updateGamesService = updateGamesService;
        }

        [FunctionName("UpdateScoresHttp")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req)
        {
            await _updateGamesService.UpdateGames();

            return new OkResult();
        }
    }
}