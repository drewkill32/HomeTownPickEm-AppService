using System;
using System.Collections.Generic;
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
    public class SelectTeamCommand : List<SelectTeamCommand.SelectTeamItem>, IRequest<IEnumerable<PickDto>>
    {
        public class SelectTeamItem
        {
            public int? SelectedTeamId { get; set; }

            public int PickId { get; set; }
        }
    }

    public class SelectTeamCommandHandler : IRequestHandler<SelectTeamCommand, IEnumerable<PickDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SelectTeamCommandHandler> _logger;
        private readonly IUserAccessor _userAccessor;

        public SelectTeamCommandHandler(ApplicationDbContext context,
            IUserAccessor userAccessor,
            ILogger<SelectTeamCommandHandler> logger)
        {
            _context = context;
            _userAccessor = userAccessor;
            _logger = logger;
        }

        public async Task<IEnumerable<PickDto>> Handle(SelectTeamCommand request, CancellationToken cancellationToken)
        {
            var user = (await _userAccessor.GetCurrentUserAsync())
                .GuardAgainstNotFound("No current user found");

            var pickIds = request.Select(x => x.PickId).ToArray();

            var picks = await _context.Pick
                .Where(x => pickIds.Contains(x.Id))
                .ToArrayAsync(cancellationToken);

            GuardAgainstForbiddenAccess(picks, user);


            foreach (var pick in picks)
            {
                var singleRequest = request.Single(x => x.PickId == pick.Id);
                if (singleRequest.SelectedTeamId.HasValue)
                {
                    var team = await GetTeam(singleRequest.SelectedTeamId.Value, pick.GameId, pick.LeagueId,
                        cancellationToken);
                    pick.SelectedTeamId = team.Id;
                }
                else
                {
                    pick.SelectedTeamId = null;
                }
            }


            _context.Pick.UpdateRange(picks);
            await _context.SaveChangesAsync(cancellationToken);
            return picks.Select(x => x.ToPickDto());
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

        private void GuardAgainstForbiddenAccess(Pick[] picks, ApplicationUser user)
        {
            foreach (var pick in picks)
            {
                if (pick.UserId != user.Id)
                {
                    _logger.LogError(
                        $"The user {_userAccessor.GetCurrentUsername()} is not valid of the pick Id: {pick.Id}");
                    throw new ForbiddenAccessException();
                }
            }
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