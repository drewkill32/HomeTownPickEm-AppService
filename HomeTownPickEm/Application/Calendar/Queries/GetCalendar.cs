using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Calendar.Queries
{
    public class GetCalendarQueryHandler : IRequestHandler<GetCalendarQuery, IEnumerable<CalendarDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetCalendarQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CalendarDto>> Handle(GetCalendarQuery request,
            CancellationToken cancellationToken)
        {
            var calendars = await _context.Calendar
                .Where(x => x.Season == request.Season && x.SeasonType == request.SeasonType)
                .Where(x => x.League.Slug == request.LeagueSlug)
                .WhereWeekIs(request.Week)
                .ToArrayAsync(cancellationToken);


            return calendars.Select(x => x.ToCalendarDto())
                .OrderBy(x => x.Season)
                .ThenBy(x => x.Week)
                .ToArray();
        }
    }

    public class GetCalendarQuery : IRequest<IEnumerable<CalendarDto>>
    {
        public string Season { get; set; } = DateTime.Now.Year.ToString();
        public string SeasonType { get; set; } = "regular";
        public string LeagueSlug { get; set; }
        public int? Week { get; set; }
    }
}