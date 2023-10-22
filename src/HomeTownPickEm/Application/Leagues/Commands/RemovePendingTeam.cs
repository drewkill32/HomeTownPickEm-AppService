using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands;

public class RemovePendingTeam
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

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var pendingInvites = await _context.PendingInvites
                .AsTracking()
                .Where(x => x.LeagueId == request.LeagueId
                            && x.Season == request.Season
                            && x.TeamId == request.TeamId)
                .ToArrayAsync(cancellationToken);

            foreach (var pendingInvite in pendingInvites)
            {
                pendingInvite.TeamId = null;
            }

            await _context.SaveChangesAsync(cancellationToken);
  
        }
    }
}