using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Picks.Commands;

public class SelectTiebreaker
{
    public class Command : IRequest
    {
        public int TotalPoints { get; set; }

        public Guid WeeklyGameId { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly ISystemDate _systemDate;
        private readonly ApplicationDbContext _context;
        private readonly IUserAccessor _userAccessor;


        public Handler(ISystemDate systemDate, ApplicationDbContext context, IUserAccessor userAccessor)
        {
            _systemDate = systemDate;
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


            var game = await _context.Games.FirstOrDefaultAsync(x => x.Id == weeklyGame.GameId, cancellationToken);

            var cutOffDate = game.StartDate.AddMinutes(-1);
            var currDate = _systemDate.UtcNow;

            if (_systemDate.UtcNow > game.StartDate)
            {
                throw new BadRequestException(
                    $"The current time {currDate:f} is past the cutoff {cutOffDate:f}");
            }

            {
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
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}