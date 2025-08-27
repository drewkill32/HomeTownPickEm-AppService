using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using HomeTownPickEm.Json;
using HomeTownPickEm.Models;
using HomeTownPickEm.Services.CFBD;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Teams.Commands.LoadTeams
{
    public class LoadTeams
    {
        public class Command : IRequest
        {
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDbContext _context;
            private readonly ILogger<Handler> _logger;
            private readonly HttpClient _httpClient;

            public Handler(IHttpClientFactory httpClientFactory, ApplicationDbContext context, ILogger<Handler> logger)
            {
                _context = context;
                _logger = logger;
                _httpClient = httpClientFactory.CreateClient(CfbdSettings.SettingsKey);
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var teamsResponse = await _httpClient.GetFromJsonAsync<IEnumerable<TeamResponse>>("/teams",
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = new SnakeCaseNamingPolicy()
                    }, cancellationToken) ?? throw new InvalidOperationException("Thee return value was null");

                var t = teamsResponse.ToHashSet(new IdEqualityComparer<TeamResponse>());
                var teams = t.Select(MapToTeam).ToArray();

                await UpdateTeams(teams, cancellationToken);
                _logger.LogInformation("Loaded {Count} teams", teams.Length);
            }

            private async Task UpdateTeams(Team[] teams, CancellationToken cancellationToken)
            {
                if (_context.Teams.Any())
                {
                    var teamIds = teams
                        .Select(x => x.Id)
                        .ToArray();

                    _context.Teams.AddOrUpdateRange(teams);

                    var toDelete = await _context.Teams
                        .Where(x => !teamIds.Contains(x.Id))
                        .ToArrayAsync(cancellationToken);
                    _context.Teams.RemoveRange(toDelete);
                }
                else
                {
                    _context.Teams.AddRange(teams);
                }

                await _context.SaveChangesAsync(cancellationToken);
            }


            private Team MapToTeam(TeamResponse teamResponse)
            {
                return new()
                {
                    Abbreviation = teamResponse.Abbreviation,
                    Color = teamResponse.Color,
                    Conference = teamResponse.Conference,
                    Division = teamResponse.Division,
                    Mascot = teamResponse.Mascot,
                    Id = teamResponse.Id,
                    Logos = teamResponse.Logos == null || teamResponse.Logos.Length == 0
                        ? ""
                        : teamResponse.Logos[0].Replace("http://", "https://"),
                    School = teamResponse.School,
                    AltColor = teamResponse.AltColor
                };
            }
        }
    }

    [DebuggerDisplay("[{Id}] {School} {Mascot}")]
    public class TeamResponse:IHasId
    {
        public int Id { get; set; }
        public string School { get; set; }
        public string Mascot { get; set; }
        public string Abbreviation { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
        public string Color { get; set; }
        [JsonPropertyName("alternateColor")]
        public string AltColor { get; set; }
        public string[] Logos { get; set; }
    }
}