using System;
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
    public class SelectTeam
    {
        public class Command : IRequest<PickDto>
        {
            public int SelectedTeam { get; set; }

            public string UserId { get; set; }
            public int Id { get; set; }
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
                var pick = (await _context.Pick
                        .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken))
                    .GuardAgainstNotFound(request.Id);

                if (pick.UserId != request.UserId)
                {
                    throw new ForbiddenAccessException();
                }

                var team = await GetTeam(request, pick.GameId, cancellationToken);

                pick.SelectedTeamId = team.Id;
                
                await _context.SaveChangesAsync(cancellationToken);
                pick.SelectedTeam = team;
                return pick.ToPickDto();
            }

            private async Task<Team> GetTeam(Command request, int gameId, CancellationToken cancellationToken)
            {
                var selectedTeam = (await _context.Teams
                        .SingleOrDefaultAsync(x => x.Id == request.SelectedTeam, cancellationToken))
                    .GuardAgainstNotFound(request.SelectedTeam);

                var game = (await _context.Games
                        .SingleOrDefaultAsync(x => x.Id == gameId, cancellationToken))
                    .GuardAgainstNotFound(gameId);

                GuardAgainstTeamNotPlaying(request.SelectedTeam, game);

                GuardAgainstPickPastCutoff(game);

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
                    throw new BadRequestException("You picked a team that is not playing this game");
                }
            }
        }
    }
}