using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Json;

namespace HomeTownPickEm.Services.Cfbd
{
    public class CfbdHttpClient : ICfbdHttpClient
    {
        private readonly HttpClient _httpClient;

        public CfbdHttpClient(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<IEnumerable<GameResponse>> GetGames(GameRequest request, CancellationToken cancellationToken)
        {
            return await _httpClient
                .GetFromJsonAsync<IEnumerable<GameResponse>>($"/games?{request.ToQueryString()}",
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = new SnakeCaseNamingPolicy()
                    }, cancellationToken) ?? throw new InvalidOperationException("The returned value was null");
        }
    }
}