using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using HomeTownPickEm.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Calendar.Queries
{
    public class GetCalendar
    {
        public class Query : IRequest<IEnumerable<CalendarDto>>
        {
            public string Season { get; set; } = DateTime.Now.Year.ToString();
            public string SeasonType { get; set; } = "regular";
            public string LeagueSlug { get; set; }
            public int? Week { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<CalendarDto>>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<CalendarDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                // cannot group by date directly in the query due to sqlite limitations
                // see https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
                var games = await _context.Games
                    .Where(x => x.Season == request.Season && x.SeasonType == request.SeasonType)
                    .Where(x => x.Picks.Any(p => p.League.Slug == request.LeagueSlug))
                    .WhereWeekIs(request.Week)
                    .Select(x => new
                    {
                        x.Week,
                        x.Season,
                        x.SeasonType,
                        x.StartDate
                    })
                    .ToArrayAsync(cancellationToken);

                var calendars =
                    games.GroupBy(x => new { x.Week, x.Season, x.SeasonType })
                        .Select(x => new CalendarDto
                        {
                            Week = x.Key.Week,
                            Season = x.Key.Season,
                            SeasonType = x.Key.SeasonType,
                            CutoffDate = x.Min(g => g.StartDate).GetLastThusMidnight(),
                            FirstGameStart = x.Min(g => g.StartDate),
                            LastGameStart = x.Max(g => g.StartDate)
                        })
                        .OrderBy(x => x.Season)
                        .ThenBy(x => x.Week)
                        .ToArray();
                return calendars;
            }
        }
    }
}