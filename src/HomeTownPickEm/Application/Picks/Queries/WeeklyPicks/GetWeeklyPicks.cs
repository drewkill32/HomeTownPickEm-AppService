using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Picks.Queries.WeeklyPicks
{
    public class GetWeeklyPicks
    {
        public class Query : IRequest<IEnumerable<WeeklyPicksDto>>
        {
            public string LeagueSlug { get; set; }

            public int? Week { get; set; }

            public string Season { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<WeeklyPicksDto>>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<WeeklyPicksDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var picks = await (from p in _context.Pick
                    join u in _context.Users on p.UserId equals u.Id
                    join g in _context.Games on p.GameId equals g.Id
                    join t in _context.Teams on u.TeamId equals t.Id
                    where p.Season.League.Slug == request.LeagueSlug && p.Season.Year == request.Season &&
                          (!request.Week.HasValue || g.Week == request.Week)
                    group p by new { g.Week, u.ProfileImg, FirstName = u.Name.First, LastName = u.Name.Last }
                    into g
                    select new WeeklyPicksDto
                    {
                        Week = g.Key.Week,
                        ProfileImg = g.Key.ProfileImg,
                        UserFirstName = g.Key.FirstName,
                        UserLastName = g.Key.LastName,
                        TotalPicks = g.Count(),
                        UnselectedPicks = g.Count(x => x.SelectedTeamId == null)
                    }).ToArrayAsync(cancellationToken);

                return picks.OrderBy(x => x.Week)
                    .ThenByDescending(x => x.UnselectedPicks)
                    .ThenBy(x => x.UserFirstName)
                    .ThenBy(x => x.UserLastName)
                    .ToArray();
            }
        }
    }
}