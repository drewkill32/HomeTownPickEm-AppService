using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Picks.Commands;

public class SelectWeeklyPicks
{
    public class Command : IRequest
    {
        public int TotalPoints { get; set; }

        public Guid WeeklyGameId { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserAccessor _userAccessor;


        public Handler(ApplicationDbContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var user = (await _userAccessor.GetCurrentUserAsync())
                .GuardAgainstNotFound("No current user found");

            var weeklyGame = (await _context.WeeklyGames.Where(x => x.Id == request.WeeklyGameId)
                    .FirstOrDefaultAsync(cancellationToken))
                .GuardAgainstNotFound("There is no Weekly Pick for that week");

            var pick = await _context.WeeklyGamePicks
                .Where(x => x.WeeklyGameId == request.WeeklyGameId && x.UserId == user.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (pick is not null)
            {
                pick.TotalPoints = request.TotalPoints;
                _context.WeeklyGamePicks.Update(pick);
            }
            else
            {
                _context.WeeklyGamePicks.Add(new()
                    {
                        TotalPoints = request.TotalPoints,
                        UserId = user.Id,
                        WeeklyGameId = request.WeeklyGameId,
                        GameId = weeklyGame.GameId
                    }
                );
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}