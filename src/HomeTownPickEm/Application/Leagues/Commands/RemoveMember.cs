using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands;

public class RemoveMember
{
    public class Command : ILeagueCommissionerRequest
    {
        public int LeagueId { get; set; }
        public string Season { get; set; }
        public string MemberId { get; set; }
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
                .Include(x => x.Members)
                .Include(x => x.Picks)
                .Where(x => x.LeagueId == request.LeagueId && x.Year == request.Season)
                .Where(x => x.Members.Any(y => y.Id == request.MemberId))
                .FirstOrDefaultAsync(cancellationToken)
                .GuardAgainstNotFound("Season not found");

            var member = season.Members.FirstOrDefault(x => x.Id == request.MemberId);

            if (member == null)
            {
                throw new NotFoundException("Member not found");
            }

            season.RemoveMember(member);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}