using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Picks.Queries
{
    public class GetPicks
    {
        public class Query : IRequest<IEnumerable<PickDto>>
        {
            public string UserId { get; set; }


            public int? Week { get; set; }

            public int LeagueId { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<PickDto>>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<PickDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var picks = await _context.Pick
                    .Where(x => x.LeagueId == request.LeagueId)
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
                    .ThenBy(x => x.User.LastName)
                    .ToArray();
                return orderedPicks.Select(x => x.ToPickDto());
            }
        }
    }
}
