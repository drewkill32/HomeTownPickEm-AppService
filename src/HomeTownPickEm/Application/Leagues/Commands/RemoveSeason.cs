using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands;

public class RemoveSeason
{
    public class Command : ILeagueCommissionerRequest
    {
        public int LeagueId { get; set; }
        public string Season { get; set; }
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
                .Include(x=> x.Members)
                .Include(x => x.Teams)
                .Include(x => x.Picks)
                .ThenInclude(p => p.Game)
                .Where(x => x.LeagueId == request.LeagueId && x.Year == request.Season)
                .FirstOrDefaultAsync(cancellationToken)
                .GuardAgainstNotFound("Season not found");


            _context.Season.Remove(season);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
