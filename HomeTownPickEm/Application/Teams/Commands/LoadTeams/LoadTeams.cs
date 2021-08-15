using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using HomeTownPickEm.Json;
using HomeTownPickEm.Models;
using MediatR;

namespace HomeTownPickEm.Application.Teams.Commands.LoadTeams
{
    public class LoadTeams
    {
        public class Command : IRequest<IEnumerable<TeamDto>>
        {
        }

        public class Handler : IRequestHandler<Command, IEnumerable<TeamDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly HttpClient _httpClient;

            public Handler(IHttpClientFactory httpClientFactory, ApplicationDbContext context)
            {
                _context = context;
                _httpClient = httpClientFactory.CreateClient(CFBDSettings.SettingsKey);
            }

            public async Task<IEnumerable<TeamDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var teamsResponse = await _httpClient.GetFromJsonAsync<IEnumerable<TeamResponse>>("/teams",
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = new SnakeCaseNamingPolicy()
                    }, cancellationToken);

                var teams = teamsResponse.Select(MapToTeam);

                if (_context.Teams.Any())
                {
                    _context.Teams.UpdateRange(teams);
                }
                else
                {
                    _context.Teams.AddRange(teams);
                }

                await _context.SaveChangesAsync(cancellationToken);
                return teams.Select(MapToDto).ToArray();
            }

            private TeamDto MapToDto(Team team)
            {
                return new()
                {
                    Abbreviation = team.Abbreviation,
                    Color = team.Color,
                    Conference = team.Conference,
                    Division = team.Division,
                    Mascot = team.Mascot,
                    Id = team.Id,
                    Logos = team.Logos.Split(';', StringSplitOptions.RemoveEmptyEntries),
                    School = team.School,
                    AltColor = team.AltColor
                };
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
                    Logos = teamResponse.Logos == null ? "" : string.Join(";", teamResponse.Logos),
                    School = teamResponse.School,
                    AltColor = teamResponse.AltColor
                };
            }
        }
    }

    [DebuggerDisplay("[{Id}] {School} {Mascot}")]
    public class TeamResponse
    {
        public int Id { get; set; }
        public string School { get; set; }
        public string Mascot { get; set; }
        public string Abbreviation { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
        public string Color { get; set; }
        public string AltColor { get; set; }
        public string[] Logos { get; set; }
    }
}