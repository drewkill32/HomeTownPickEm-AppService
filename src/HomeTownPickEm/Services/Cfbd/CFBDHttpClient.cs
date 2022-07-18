using System.Text.Json;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Json;
using Polly;
using Polly.Retry;

namespace HomeTownPickEm.Services.Cfbd
{
    public class CfbdHttpClient : ICfbdHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy<IEnumerable<GameResponse>> _policy;

        public CfbdHttpClient(HttpClient client)
        {
            _httpClient = client;
            _policy = Policy<IEnumerable<GameResponse>>
                .Handle<Exception>()
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                    retryAttempt)));
        }

        public async Task<IEnumerable<GameResponse>> GetGames(GameRequest request, CancellationToken cancellationToken)
        {
            return await _policy.ExecuteAsync(token => _httpClient
                .GetFromJsonAsync<IEnumerable<GameResponse>>($"/games?{request.ToQueryString()}",
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = new SnakeCaseNamingPolicy()
                    }, token), cancellationToken) ?? throw new InvalidOperationException("The returned value was null");
        }
    }
}