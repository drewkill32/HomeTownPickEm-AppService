using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Games.Queries
{
    public class GetGameQueryHandler : IRequestHandler<GetGameQuery, GameDto>
    {
        private readonly ApplicationDbContext _context;

        public GetGameQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GameDto> Handle(GetGameQuery request, CancellationToken cancellationToken)
        {
            var game = await _context.Games.Where(x => x.Id == request.Id)
                .Include(x => x.Away)
                .Include(x => x.Home)
                .AsSingleQuery()
                .SingleOrDefaultAsync(cancellationToken);
            return game.ToGameDto();
        }
    }

    public class GetGameQuery : IRequest<GameDto>
    {
        public int Id { get; set; }
    }
}