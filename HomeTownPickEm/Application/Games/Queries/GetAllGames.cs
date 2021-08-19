using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using HomeTownPickEm.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Games.Queries
{
    public class GetAllGames
    {
        public class Query : IRequest<IEnumerable<GameDto>>
        {
            public string Season { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<GameDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly GameTeamRepository _gameTeamRepository;

            public QueryHandler(ApplicationDbContext context, GameTeamRepository service)
            {
                _context = context;
                _gameTeamRepository = service;
            }

            public async Task<IEnumerable<GameDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var games = await _context.Games
                    .Where(x => x.Season == request.Season)
                    .ToArrayAsync(cancellationToken);
                await _gameTeamRepository.LoadTeamCollection(games, cancellationToken);
                return games.Select(x => _gameTeamRepository.MapToDto(x))
                    .OrderBy(x => x.Week).ThenBy(x => x.StartDate).ToArray();
            }
        }
    }
}