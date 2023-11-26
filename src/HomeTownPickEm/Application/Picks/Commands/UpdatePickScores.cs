using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using HomeTownPickEm.Services.Cfbd;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Picks.Commands
{
    public class UpdatePickScores
    {
        public class Command : GameRequest, IRequest
        {
            public string LeagueSlug { get; set; }
            
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly ApplicationDbContext _context;
            private readonly ICfbdHttpClient _httpClient;

            public CommandHandler(ICfbdHttpClient httpClient, ApplicationDbContext context)
            {
                _httpClient = httpClient;
                _context = context;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var completedGames = await GetCompletedGames(request, cancellationToken);

                var picks = await GetPicks(request, completedGames, cancellationToken);

                UpdatePicks(picks, completedGames);

                await _context.SaveChangesAsync(cancellationToken);
                
            }

            private async Task<Game[]> GetCompletedGames(Command request, CancellationToken cancellationToken)
            {
                var completedGamesResponse = (await _httpClient.GetGames(request, cancellationToken))
                    .Where(x => x.GameFinal)
                    .ToArray();

                var completedIds = completedGamesResponse.Select(x => x.Id).ToArray();

                var completedGames = await _context.Games
                    .Where(x => completedIds.Contains(x.Id))
                    .AsTracking()
                    .ToArrayAsync(cancellationToken);

                foreach (var game in completedGames)
                {
                    var res = completedGamesResponse.Single(x => x.Id == game.Id);
                    game.HomePoints = res.HomePoints;
                    game.AwayPoints = res.AwayPoints;
                }

                return completedGames;
            }

            private async Task<Pick[]> GetPicks(Command request, Game[] completedGames,
                CancellationToken cancellationToken)
            {
                var gameIds = completedGames.Select(x => x.Id).ToArray();

                return await _context.Pick
                    .Where(p => gameIds.Contains(p.GameId) &&
                                p.Season.League.Slug == request.LeagueSlug &&
                                p.Season.Year == request.Year)
                    .AsTracking()
                    .ToArrayAsync(cancellationToken);
            }

            private static void UpdatePick(Game[] completedGames, Pick pick)
            {
                var game = completedGames.Single(x => x.Id == pick.GameId);
                var winner = game.WinnerId;
                pick.Points = pick.SelectedTeamId == winner ? 1 : 0;
            }

            private static void UpdatePicks(Pick[] picks, Game[] completedGames)
            {
                foreach (var pick in picks)
                {
                    UpdatePick(completedGames, pick);
                }
            }
        }
    }
}