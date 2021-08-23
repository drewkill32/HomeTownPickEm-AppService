using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
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
        }
        
        public class QueryHandler: IRequestHandler<Query,IEnumerable<CalendarDto>>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }
            
            public async Task<IEnumerable<CalendarDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var calendars = await _context.Calendar.Where(x => 
                        x.Season == request.Season && x.SeasonType == request.SeasonType)
                    .ToArrayAsync(cancellationToken);

                return calendars.Select(x => x.ToCalendarDto());
            }
        }
    }
}