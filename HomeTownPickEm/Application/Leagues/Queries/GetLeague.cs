using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Queries
{
    public class GetLeague
    {
        public class Query : IRequest<LeagueDto>
        {
            public string Name { get; set; }
            public string Year { get; set; }

            public bool IncludePicks { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, LeagueDto>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<LeagueDto> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<League> query = _context.League.Where(x => x.Name == request.Name)
                    .Include(x => x.Teams)
                    .Include(x => x.Members);
                if (request.IncludePicks)
                {
                    query = query.Include(x => x.Picks);
                }

                var league = await query.AsSplitQuery()
                    .FirstOrDefaultAsync(cancellationToken);

                if (league == null)
                {
                    throw new NotFoundException($"No League found with name: {request.Name} and year {request.Year}");
                }

                var dto = league.ToLeagueDto();
                return dto;
            }
        }
    }
}