using System;
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
            private readonly IUserAccessor _userAccessor;
            private readonly ILogger<CommandHandler> _logger;

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
                    _logger.LogError($"The user {_userAccessor.GetCurrentUsername()} is not valid of the pick Id: {pick.Id}");
                    throw new ForbiddenAccessException();
                }
                
                if (request.SelectedTeam.HasValue)
                {
                    var team = await GetTeam(request.SelectedTeam.Value, pick.GameId, cancellationToken);

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

            private async Task<Team> GetTeam(int teamId, int gameId, CancellationToken cancellationToken)
            {
                var game = (await _context.Games
                        .SingleOrDefaultAsync(x => x.Id == gameId, cancellationToken))
                    .GuardAgainstNotFound(gameId);

                GuardAgainstPickPastCutoff(game);

                var selectedTeam = (await _context.Teams
                        .SingleOrDefaultAsync(x => x.Id == teamId, cancellationToken))
                    .GuardAgainstNotFound(teamId);

                GuardAgainstTeamNotPlaying(teamId, game);


                return selectedTeam;
            }

            private static void GuardAgainstPickPastCutoff(Game game)
            {
                var prevThurs = game.StartDate.GetLastThusMidnight();
                var currDate = DateTimeOffset.UtcNow;
                if (currDate > prevThurs)
                {
                    throw new BadRequestException(
                        $"The current time {currDate:f} is past the cutoff {prevThurs:f}");
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