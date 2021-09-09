using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeTownPickEm.Application.Picks.Commands
{
    public class SelectTeam
    {
        public class Command : IRequest<PickDto>
        {
            public int? SelectedTeam { get; set; }

            public int Id { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, PickDto>
        {
            private readonly ApplicationDbContext _context;
            private readonly ILogger<CommandHandler> _logger;
            private readonly IUserAccessor _userAccessor;

            public CommandHandler(ApplicationDbContext context,
                IUserAccessor userAccessor,
                ILogger<CommandHandler> logger)
            {
                _context = context;
                _userAccessor = userAccessor;
                _logger = logger;
            }

            public async Task<PickDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var pick = (await _context.Pick
                        .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken))
                    .GuardAgainstNotFound(request.Id);

                var user = (await _userAccessor.GetCurrentUserAsync())
                    .GuardAgainstNotFound("No current user found");

                if (pick.UserId != user.Id)
                {
                    _logger.LogError(
                        $"The user {_userAccessor.GetCurrentUsername()} is not valid of the pick Id: {pick.Id}");
                    throw new ForbiddenAccessException();
                }

                if (request.SelectedTeam.HasValue)
                {
                    var team = await GetTeam(request.SelectedTeam.Value, pick.GameId, pick.LeagueId, cancellationToken);

                    pick.SelectedTeamId = team.Id;
                }
                else
                {
                    pick.SelectedTeamId = null;
                }


                _context.Update(pick);
                await _context.SaveChangesAsync(cancellationToken);
                return pick.ToPickDto();
            }

            private async Task<Team> GetTeam(int teamId, int gameId, int leagueId, CancellationToken cancellationToken)
            {
                var game = (await _context.Games
                        .SingleOrDefaultAsync(x => x.Id == gameId, cancellationToken))
                    .GuardAgainstNotFound(gameId);

                await GuardAgainstPickPastCutoff(game, leagueId);

                var selectedTeam = (await _context.Teams
                        .SingleOrDefaultAsync(x => x.Id == teamId, cancellationToken))
                    .GuardAgainstNotFound(teamId);

                GuardAgainstTeamNotPlaying(teamId, game);


                return selectedTeam;
            }

            private async Task GuardAgainstPickPastCutoff(Game game, int leagueId)
            {
                var cutOffDate = await _context.Calendar
                    .Where(x => x.Week == game.Week && x.LeagueId == leagueId)
                    .Select(x => x.CutoffDate)
                    .SingleOrDefaultAsync();
                var currDate = DateTimeOffset.UtcNow;
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
}