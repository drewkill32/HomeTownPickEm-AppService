using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leaderboard.Queries
{
    public class GetLeaderboard
    {
        public class Query : IRequest<IEnumerable<LeaderBoardDto>>
        {
            public string LeagueSlug { get; set; }
            public string Season { get; set; } = DateTime.Now.Year.ToString();
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
                    .Where(x => x.LeagueSlug == request.LeagueSlug && x.Year == request.Season)
                    .OrderByDescending(x => x.TotalPoints)
                    .ThenBy(x => x.UserFirstName)
                    .ThenBy(x => x.UserLastName)
                    .ToArrayAsync(cancellationToken);

                return leaderboard.Select(x => x.ToLeaderBoardDto());
            }
        }
    }
}