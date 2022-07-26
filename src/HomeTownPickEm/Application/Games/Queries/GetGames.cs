using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using HomeTownPickEm.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Games.Queries
{
    public class GetGames
    {
        public class Query : IRequest<IEnumerable<GameDto>>
        {
            public string Season { get; set; }

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
                var games = await _context.Games
                    .Where(x => x.Season == request.Season)
                    .WhereTeamIsPlaying(request.TeamId)
                    .WhereWeekIs(request.Week)
                    .Include(x => x.Away)
                    .Include(x => x.Home)
                    .ToArrayAsync(cancellationToken);

                await _repository.LoadTeamCollection(games, cancellationToken);
                
                
                return games.Select(x => _repository.MapToDto(x))
                    .OrderBy(x => x.Week)
                    .ThenBy(x => x.StartDate)
                    .ThenBy(x => x.AwayTeam.Name)
                    .ToArray();
                
            }
        }
    }
}