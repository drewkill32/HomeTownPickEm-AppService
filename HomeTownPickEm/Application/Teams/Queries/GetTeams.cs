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
    public class GetTeamsQueryHandler : IRequestHandler<GetTeamsQuery, IEnumerable<TeamDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetTeamsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeamDto>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
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

    public class GetTeamsQuery : IRequest<IEnumerable<TeamDto>>
    {
        public bool IncludeNoConference { get; set; }

        public string Conference { get; set; }


        public string Name { get; set; }
        public int? Top { get; set; }
    }
}