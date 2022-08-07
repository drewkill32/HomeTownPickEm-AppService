using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using HomeTownPickEm.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands
{
    public class AddNewTeam
    {
        public class Command : ILeagueCommissionerRequest
        {
            public int LeagueId { get; set; }
            public string Season { get; set; }
            public int TeamId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, Unit>
        {
            private readonly ApplicationDbContext _context;


            public CommandHandler(ApplicationDbContext context)
            {
                _context = context;
            
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var season = await _context.Season
                    .Where(x => x.LeagueId == request.LeagueId && x.Year == request.Season)
                    .Include(x => x.Teams)
                    .Include(x => x.Members)
                    .AsTracking()
                    .FirstOrDefaultAsync(cancellationToken)
                    .GuardAgainstNotFound(
                        $"There is no season with for {request.Season} and LeagueId {request.LeagueId}");
                ;

                var team = await _context.Teams
                    .AsTracking()
                    .Where(x => x.Id == request.TeamId)
                    .FirstOrDefaultAsync(cancellationToken);

                var games = await _context.Games.WhereTeamIsPlaying(team)
                    .Where(g => g.Season == request.Season)
                    .AsTracking()
                    .ToArrayAsync(cancellationToken);

                season.AddTeam(team, games);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}