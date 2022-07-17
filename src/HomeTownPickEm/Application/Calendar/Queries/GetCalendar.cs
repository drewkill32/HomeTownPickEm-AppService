using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
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
            public int? Week { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<CalendarDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IEnumerable<CalendarDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var calendars = await _context.Calendar
                    .Where(x => x.Season == request.Season && x.SeasonType == request.SeasonType)
                    .WhereWeekIs(request.Week)
                    .ProjectTo<CalendarDto>(_mapper.ConfigurationProvider)
                    .ToArrayAsync(cancellationToken);


                return calendars
                    .OrderBy(x => x.Season)
                    .ThenBy(x => x.Week)
                    .ToArray();
            }
        }
    }
}