using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leaderboard.Queries
{
    public class GetLeaderboard
    {
        public class Query : IRequest<IEnumerable<LeaderBoardDto>>
        {
            public string Slug { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<LeaderBoardDto>>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<LeaderBoardDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var leaderboard = await _context.Leaderboard
                    .Where(x => x.LeagueSlug == request.Slug)
                    .ToArrayAsync(cancellationToken);

                return leaderboard.Select(x => x.ToLeaderBoardDto());
            }
        }
    }
}