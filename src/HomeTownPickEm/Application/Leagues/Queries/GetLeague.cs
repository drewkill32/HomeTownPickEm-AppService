using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Queries
{
    public class GetLeague
    {
        public class Query : IRequest<object>
        {
            public string LeagueSlug { get; set; }
            public string Season { get; set; }

        }

        public class QueryHandler : IRequestHandler<Query, object>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<object> Handle(Query request, CancellationToken cancellationToken)
            {
                var season = (await _context.Season
                        .Where(x => x.League.Slug == request.LeagueSlug && x.Year == request.Season)
                        .Select(s => new
                        {
                            Id = s.LeagueId,
                            s.League.Name,
                            s.League.Slug,
                            s.League.ImageUrl,
                            Season = s.Year
                        }).FirstOrDefaultAsync(cancellationToken))
                    .GuardAgainstNotFound($"No League found with name: {request.LeagueSlug} and year {request.Season}");


                return season;
            }
        }
    }
}