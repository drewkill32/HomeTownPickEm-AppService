using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Teams.Commands;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Teams.Queries
{
    public class GetAll
    {
        public class Query : IRequest<IEnumerable<TeamDto>>
        {
            
            
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
                var teams = await _context.Teams.ToArrayAsync(cancellationToken);

                return teams.Select(x => x.ToTeamDto());
            }
        }
    }
}