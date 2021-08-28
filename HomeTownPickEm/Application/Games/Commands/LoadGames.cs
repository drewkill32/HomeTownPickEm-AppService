using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Config;
using HomeTownPickEm.Data;
using HomeTownPickEm.Json;
using HomeTownPickEm.Models;
using HomeTownPickEm.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Games.Commands
{
    public class LoadGames
    {
        public class Command : IRequest<IEnumerable<GameDto>>
        {
            public string Year { get; set; } = DateTime.Now.Year.ToString();
            public int? Week { get; set; }
            public string SeasonType { get; set; } = "regular";
            public int? TeamId { get; set; }
        }

        public class Handler : IRequestHandler<Command, IEnumerable<GameDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly HttpClient _httpClient;
            private readonly GameTeamRepository _repository;

            public Handler(IHttpClientFactory httpClientFactory, ApplicationDbContext context,
                GameTeamRepository repository)
            {
                _context = context;
                _repository = repository;
                _httpClient = httpClientFactory.CreateClient(CFBDSettings.SettingsKey);
            }

            public async Task<IEnumerable<GameDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var query = await GetQueryString(request, cancellationToken);

                var gamesResponse = await _httpClient.GetFromJsonAsync<IEnumerable<GameResponse>>($"/games?{query}",
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = new SnakeCaseNamingPolicy()
                    }, cancellationToken) ?? throw new InvalidOperationException("The returned value was null");

                var games = gamesResponse.Select(MapToGame).ToArray();

                if (_context.Games.Any())
                {
                    _context.Games.UpdateRange(games);
                }
                else
                {
                    _context.Games.AddRange(games);
                }

                await _context.SaveChangesAsync(cancellationToken);
                await _repository.LoadTeamCollection(games, cancellationToken);

                return games.Select(x => _repository.MapToDto(x)).ToArray();
            }

            private async Task<string> GetQueryString(Command request, CancellationToken cancellationToken)
            {
                var teamName = request.TeamId.HasValue
                    ? await _context.Teams.Where(x => x.Id == request.TeamId)
                        .Select(x => x.School)
                        .FirstAsync(cancellationToken)
                    : "";

                return $"year={request.Year}&seasonType={request.SeasonType}";
            }


            private Game MapToGame(GameResponse gameResponse)
            {
                return new Game
                {
                    Id = gameResponse.Id,
                    Season = gameResponse.Season.ToString(),
                    Week = gameResponse.Week,
                    AwayId = gameResponse.AwayId,
                    AwayPoints = gameResponse.AwayPoints,
                    HomeId = gameResponse.HomeId,
                    HomePoints = gameResponse.HomePoints,
                    SeasonType = gameResponse.SeasonType,
                    StartTimeTbd = gameResponse.StartTimeTbd,
                    StartDate = gameResponse.StartDate
                };
            }
        }
    }

    public class GameResponse
    {
        public int Id { get; set; }

        public int Season { get; set; }

        public int Week { get; set; }

        public string SeasonType { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public bool StartTimeTbd { get; set; }

        public int HomeId { get; set; }

        public int? HomePoints { get; set; }

        public int AwayId { get; set; }

        public int? AwayPoints { get; set; }
    }
}