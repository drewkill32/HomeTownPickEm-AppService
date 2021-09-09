using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Picks.Queries
{
    public class GetPicks
    {
        public class Query : IRequest<PicksCollection>
        {
            public string UserId { get; set; }

            public int? Week { get; set; }

            public string LeagueSlug { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, PicksCollection>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<PicksCollection> Handle(Query request, CancellationToken cancellationToken)
            {
                var picks = await _context.Pick
                    .Where(x => x.League.Slug == request.LeagueSlug)
                    .WhereUserIs(request.UserId)
                    .WhereWeekIs(request.Week)
                    .Include(x => x.Game)
                    .Include(x => x.Game.Away)
                    .Include(x => x.Game.Home)
                    .Include(x => x.User)
                    .Include(x => x.SelectedTeam)
                    .AsSplitQuery()
                    .ToArrayAsync(cancellationToken);


                var orderedPicks = picks
                    .OrderBy(x => x.Game.Week)
                    .ThenBy(x => x.Game.StartDate)
                    .ThenBy(x => x.Game.Home.School)
                    .ThenBy(x => x.Game.Home.Mascot)
                    .ToArray();
                var cutOffDate = await _context.Calendar
                                     .Where(x => x.Week == request.Week && x.League.Slug == request.LeagueSlug)
                                     .Select(x => x.CutoffDate)
                                     .SingleOrDefaultAsync(cancellationToken) ??
                                 throw new NullReferenceException("The cutoff was not found");
                return orderedPicks.ToPicksDto(cutOffDate);
            }
        }
    }
}