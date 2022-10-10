using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Seasons.Queries;

public class CurrentSeason
{
    public class Query : IRequest<SeasonDto>
    {
    }

    public class Handler : IRequestHandler<Query, SeasonDto>
    {
        private readonly ISystemDate _date;
        private readonly ApplicationDbContext _context;

        public Handler(ISystemDate date, ApplicationDbContext context)
        {
            _date = date;
            _context = context;
        }

        public async Task<SeasonDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var year = _date.Now.Year.ToString();
            var dates = (await _context.Calendar.Where(x => x.Season == year)
                    .ToArrayAsync(cancellationToken))
                .OrderBy(x => x.FirstGameStart)
                .ToArray();

            var minDate = dates.Min(x => x.FirstGameStart);
            var lastDate = dates.Max(x => x.LastGameStart);


            var week = (from cal in dates
                    where DateOnly.FromDateTime(_date.UtcNow.DateTime) <=
                          DateOnly.FromDateTime(cal.LastGameStart.DateTime)
                    select cal.Week)
                .FirstOrDefault();

            return new SeasonDto
            {
                Season = year,
                FirstGameStart = minDate,
                LastGameStart = lastDate,
                Week = week
            };
        }
    }
}