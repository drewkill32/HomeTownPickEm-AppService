using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leaderboard.Queries
{
    public class GetLeaderboardQueryHandler : IRequestHandler<GetLeaderboardQuery, IEnumerable<LeaderBoardDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetLeaderboardQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaderBoardDto>> Handle(GetLeaderboardQuery request,
            CancellationToken cancellationToken)
        {
            var leaderboard = await _context.Leaderboard
                .Where(x => x.LeagueSlug == request.Slug)
                .OrderByDescending(x => x.TotalPoints)
                .ThenBy(x => x.UserFirstName)
                .ThenBy(x => x.UserLastName)
                .ToArrayAsync(cancellationToken);

            return leaderboard.Select(x => x.ToLeaderBoardDto());
        }
    }

    public class GetLeaderboardQuery : IRequest<IEnumerable<LeaderBoardDto>>
    {
        public string Slug { get; set; }
    }
}