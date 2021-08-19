using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using HomeTownPickEm.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Games.Queries
{
    public class GetByTeamWeek
    {
        public class Query : IRequest<IEnumerable<GameDto>>
        {
            public int? Week { get; set; }
            public int? TeamId { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<GameDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly GameTeamRepository _repository;

            public QueryHandler(ApplicationDbContext context, GameTeamRepository repository)
            {
                _context = context;
                _repository = repository;
            }

            public async Task<IEnumerable<GameDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                IQueryable<Game> query = _context.Games;
                if (request.TeamId.HasValue)
                {
                    query = query.Where(x => x.HomeId == request.TeamId || x.AwayId == request.TeamId);
                }

                if (request.Week.HasValue)
                {
                    query = query.Where(x => x.Week == request.Week);
                }

                var games = await query
                    .Include(x => x.Away)
                    .Include(x => x.Home)
                    .ToArrayAsync(cancellationToken);

                await _repository.LoadTeamCollection(games, cancellationToken);
                return games.Select(x => _repository.MapToDto(x));
            }
        }
    }
}