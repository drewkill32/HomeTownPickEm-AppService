using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands;

public class RemoveTeam
{
    public class Command : ILeagueCommissionerRequest
    {
        public int LeagueId { get; set; }
        public string Season { get; set; }
        public int TeamId { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var season = await _context.Season
                .AsTracking()
                .Include(x => x.Teams)
                .Include(x => x.Picks)
                .ThenInclude(p => p.Game)
                .Where(x => x.LeagueId == request.LeagueId && x.Year == request.Season && x.Picks.Any(p =>
                    p.Game.HomeId == request.TeamId || p.Game.AwayId == request.TeamId))
                .FirstOrDefaultAsync(cancellationToken)
                .GuardAgainstNotFound("Season not found");

            var team = season.Teams.FirstOrDefault(x => x.Id == request.TeamId);

            if (team == null)
            {
                throw new NotFoundException("Team not found");
            }

            season.RemoveTeam(team);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}