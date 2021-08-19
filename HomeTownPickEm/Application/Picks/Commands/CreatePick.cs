using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Picks.Commands
{
    public class CreatePick
    {
        public class Command : IRequest<PickDto>
        {
            public int LeagueId { get; set; }

            public int GameId { get; set; }

            public string UserId { get; set; }

            public int[] SelectedTeams { get; set; } = Array.Empty<int>();
        }

        public class CommandHandler : IRequestHandler<Command, PickDto>
        {
            private readonly ApplicationDbContext _context;

            public CommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<PickDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var game = await GetGame(request, cancellationToken);

                var pick = await _context.Pick
                    .Where(x => x.LeagueId == request.LeagueId && x.GameId == request.GameId)
                    .Include(x => x.TeamsPicked)
                    .AsTracking()
                    .SingleOrDefaultAsync(cancellationToken);

                var teams = await _context.Teams.Where(x => request.SelectedTeams.Contains(x.Id))
                    .AsTracking()
                    .ToArrayAsync(cancellationToken);

                if (pick == null)
                {
                    pick = new Pick
                    {
                        LeagueId = request.LeagueId,
                        UserId = request.UserId,
                        TeamsPicked = teams,
                        GameId = request.GameId,
                        Points = 0
                    };
                    _context.Add(pick);
                }
                else
                {
                    pick.TeamsPicked = teams;
                    pick.Points = 0;
                    _context.Update(pick);
                }

                await _context.SaveChangesAsync(cancellationToken);
                pick.Game = game;
                return pick.ToPickDto();
            }

            private async Task<Game> GetGame(Command request, CancellationToken cancellationToken)
            {
                var game = await _context.Games.SingleOrDefaultAsync(x => x.Id == request.GameId, cancellationToken);


                var leagueTeamIds = (await _context.League.Where(x => x.Id == request.LeagueId)
                    .Include(x => x.Teams)
                    .SingleOrDefaultAsync(cancellationToken)).Teams.Select(x => x.Id).ToArray();

                if (!leagueTeamIds.Intersect(new[] { game.HomeId, game.AwayId }).Any())
                {
                    throw new BadRequestException("At least one of the teams must be in the league.");
                }

                if (game == null)
                {
                    throw new NotFoundException("Game", request.GameId);
                }

                var prevThurs = game.StartDate.GetLastThusMidnight();
                var currDate = DateTimeOffset.UtcNow;
                if (currDate > prevThurs)
                {
                    throw new BadRequestException(
                        $"The current time {currDate:f} is past the cutoff {prevThurs:f}");
                }

                if (request.SelectedTeams.Length > 2)
                {
                    throw new BadRequestException("You cannot select more than two teams");
                }

                if (request.SelectedTeams.Length == 2 &&
                    leagueTeamIds.Intersect(new[] { game.HomeId, game.AwayId }).Count() !=
                    2) //must be a head-to-head matchup
                {
                    throw new BadRequestException("You cannot pick two teams unless it is a head-to-head matchup");
                }

                if (request.SelectedTeams.Except(new[] { game.HomeId, game.AwayId }).Any())
                {
                    throw new BadRequestException("You picked a team that is not playing this game");
                }

                return game;
            }
        }
    }
}