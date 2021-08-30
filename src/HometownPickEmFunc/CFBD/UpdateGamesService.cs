using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HometownPickEmFunc.Extensions;
using HometownPickEmFunc.Json;

namespace HometownPickEmFunc.CFBD
{
    public interface IUpdateGamesService
    {
        Task UpdateGames();
    }

    public class UpdateGamesService : IUpdateGamesService
    {
        private readonly HttpClient _httpClient;

        public UpdateGamesService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient(CFBDSettings.SettingsKey);
        }

        public async Task UpdateGames()
        {
            var currentYear = DateTime.Now.Year.ToString();
            //only update the current week?
            var gamesResponse = await _httpClient.GetFromJsonAsync<IEnumerable<GameResponse>>(
                $"/games?year={currentYear}&seasonType=regular",
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = new SnakeCaseNamingPolicy()
                }) ?? throw new InvalidOperationException("The returned value was null");
        }
    }
}