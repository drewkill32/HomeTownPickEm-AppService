using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Picks.Commands;

public class AdminCreatePick
{
    public class Command: ILeagueCommissionerRequest
    {
        public int SelectedTeamId { get; set; }
        
        public int GameId { get; set; }
        
        public string UserId { get; set; }
        public int LeagueId { get; set; }
    }

    public class Handler: IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            //get the user
            var user = await _context.Users.FirstOrDefaultAsync(x=> x.Id == request.UserId, cancellationToken);
            
            if (user == null)
            {
                throw new NotFoundException(nameof(user), request.UserId);
            }
            
            // get the pick from the league
            var picks = await _context.Season.Where(x =>
                    x.LeagueId == request.LeagueId)
                .SelectMany(x => x.Picks).Where(x=> x.GameId == request.GameId && x.UserId == request.UserId).ToArrayAsync(cancellationToken);

            foreach (var pick in picks)
            {
                pick.SelectedTeamId = request.SelectedTeamId;
            }
            
            _context.Pick.UpdateRange(picks);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}