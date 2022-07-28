using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using HomeTownPickEm.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands
{
    public class AddNewMember
    {
        public class Command : ILeagueCommissionerRequest
        {
            public int LeagueId { get; set; }
            public string Season { get; set; }
            
            public string UserId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly ApplicationDbContext _context;

            public CommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //get the season
                var season = await
                    _context.Season
                        .AsTracking()
                        .Include(x => x.Teams)
                        .Include(x => x.Members)
                        .Where(s => s.LeagueId == request.LeagueId && s.Year == request.Season)
                        .FirstOrDefaultAsync(cancellationToken)
                        .GuardAgainstNotFound(
                            $"There is no season with for {request.Season} and LeagueId {request.LeagueId}");

                //get the user
                var user = await _context.Users
                    .AsTracking()
                    .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken)
                    .GuardAgainstNotFound($"User '{request.UserId}' not found");


                //get the games to add
                var teamIds = season.Teams.Select(x => x.Id).ToArray();
                var games = await _context.Games.WhereTeamsArePlaying(teamIds)
                    .AsTracking()
                    .ToArrayAsync(cancellationToken);

                season.AddMember(user, games);
                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}