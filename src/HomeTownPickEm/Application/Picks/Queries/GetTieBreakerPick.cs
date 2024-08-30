using HomeTownPickEm.Data;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Picks.Queries;

public class GetTieBreakerPick
{
    public class Query : IRequest<int?>
    {
        public Guid WeeklyGameId { get; set; }
    }

    public class Queryhandler : IRequestHandler<Query, int?>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserAccessor _userAccessor;

        public Queryhandler(ApplicationDbContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<int?> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _userAccessor.GetCurrentUserAsync();

            var pick = await _context.WeeklyGamePicks.FirstOrDefaultAsync(
                x => x.WeeklyGameId == request.WeeklyGameId && x.UserId == user.Id,
                cancellationToken);

            return pick?.TotalPoints;
        }
    }
}