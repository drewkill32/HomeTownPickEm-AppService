using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Teams.Queries
{
    public class GetTeams
    {
        public class Query : IRequest<IEnumerable<TeamDto>>
        {
            public bool IncludeNoConference { get; set; }

            public string Conference { get; set; }


            public string Name { get; set; }
            public int? Top { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<TeamDto>>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<TeamDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.Teams
                    .IncludeNoConference(request.IncludeNoConference)
                    .WhereConferenceIs(request.Conference)
                    .WhereNameLike(request.Name)
                    .OrderBy(x => x.School)
                    .ThenBy(x => x.Mascot)
                    .Top(request.Top);

                var teams = await query.ToArrayAsync(cancellationToken);

                return teams.Select(x => x.ToTeamDto())
                    .OrderBy(x => x.Name)
                    .ToArray();
            }
        }
    }
}