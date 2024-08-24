using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Picks.Commands;

public class SelectTeam
{
    public class Command : IRequest<IEnumerable<PickDto>>
    {
        public int GameId { get; set; }

        public string Season { get; set; }

        public string LeagueSlug { get; set; }

        public int[] SelectedTeamIds { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command, IEnumerable<PickDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CommandHandler> _logger;
        private readonly IUserAccessor _userAccessor;
        private readonly ISystemDate _date;

        public CommandHandler(ApplicationDbContext context,
            IUserAccessor userAccessor,
            ISystemDate date,
            ILogger<CommandHandler> logger)
        {
            _context = context;
            _userAccessor = userAccessor;
            _date = date;
            _logger = logger;
        }

        public async Task<IEnumerable<PickDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = (await _userAccessor.GetCurrentUserAsync())
                .GuardAgainstNotFound("No current user found");


            var season = await _context.Season
                .Where(s => s.Year == request.Season && s.League.Slug == request.LeagueSlug)
                .Include(x => x.Teams)
                .FirstOrDefaultAsync(cancellationToken).GuardAgainstNotFound("No season found");

            var game = await _context.Games.FindAsync(request.GameId, cancellationToken);

            var seasonId = season.Id;
            var selectedTeamIds = request.SelectedTeamIds;

            var gameId = request.GameId;

            GuardAgainstInvalidRequest(request.SelectedTeamIds, season, game);

            var picks = await _context.Pick
                .Where(x => x.UserId == user.Id && x.GameId == gameId && x.SeasonId == seasonId)
                .AsTracking()
                .ToArrayAsync(cancellationToken);

            if (picks.Length > 0)
            {
                if (picks.Length == request.SelectedTeamIds.Length)
                {
                    for (var i = 0; i < picks.Length; i++)
                    {
                        picks[i].SelectedTeamId = request.SelectedTeamIds[i];
                    }

                    await _context.SaveChangesAsync(cancellationToken);

                    return picks.Select(x => x.ToPickDto());
                }

                //somehow we have more picks in the database clean it up
                _logger.LogError("The number of teams selected do not match.");
                _context.Pick.RemoveRange(picks);
                await _context.SaveChangesAsync(cancellationToken);
            }


            var newPicks = selectedTeamIds.Select(teamId => new Pick()
            {
                Points = 0,
                GameId = gameId,
                SelectedTeamId = teamId,
                SeasonId = seasonId,
                UserId = user.Id
            }).ToArray();

            _context.Pick.AddRange(newPicks);
            await _context.SaveChangesAsync(cancellationToken);
            return newPicks.Select(x => x.ToPickDto());
        }

        private static void GuardAgainstInvalidRequest(int[] selectedTeamIds, Season season, Game game)
        {
            var isHead2Head = season.IsHead2Head(game);
            if (isHead2Head && selectedTeamIds.Length != 2)
            {
                throw new BadRequestException("Head 2 Head games should have two picks");
            }

            if (!isHead2Head && selectedTeamIds.Length != 1)
            {
                throw new BadRequestException("Non Head 2 Head games should have one pick");
            }
        }

        private void GuardAgainstPickPastCutoff(Game game)
        {
            var cutOffDate = game.StartDate.AddMinutes(-1);
            var currDate = _date.UtcNow;
            if (currDate > cutOffDate)
            {
                throw new BadRequestException(
                    $"The current time {currDate:f} is past the cutoff {cutOffDate:f}");
            }
        }

        private static void GuardAgainstTeamNotPlaying(int teamId, Game game)
        {
            if (game.HomeId != teamId && game.AwayId != teamId)
            {
                throw new BadRequestException(
                    $"You picked a team that is not playing this game. GameId: {game.Id} teamId: {teamId}");
            }
        }
    }
}