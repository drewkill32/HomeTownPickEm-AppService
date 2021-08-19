using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Games.Queries
{
    public class GetGame
    {
        public class Query : IRequest<GameDto>
        {
            public int Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, GameDto>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<GameDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var game = await _context.Games.Where(x => x.Id == request.Id)
                    .Include(x => x.Away)
                    .Include(x => x.Home)
                    .AsSingleQuery()
                    .SingleOrDefaultAsync(cancellationToken);
                return game.ToGameDto();
            }
        }
    }
}