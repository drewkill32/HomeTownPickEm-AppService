using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Data;
using HomeTownPickEm.Services;
using HomeTownPickEm.Services.Cfbd;
using MediatR;

namespace HomeTownPickEm.Application.Games.Commands
{
    public class LoadGames
    {
        public class Command : GameRequest, IRequest<IEnumerable<GameDto>>
        {
        }

        public class Handler : IRequestHandler<Command, IEnumerable<GameDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly ICfbdHttpClient _httpClient;
            private readonly GameTeamRepository _repository;

            public Handler(ICfbdHttpClient httpClient, ApplicationDbContext context,
                GameTeamRepository repository)
            {
                _context = context;
                _repository = repository;
                _httpClient = httpClient;
            }

            public async Task<IEnumerable<GameDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var gamesResponse = await _httpClient.GetGames(request, cancellationToken);

                var games = gamesResponse.Select(x => x.ToGame())
                    .ToArray();

                if (_context.Games.Any())
                {
                    _context.Games.UpdateRange(games);
                }
                else
                {
                    _context.Games.AddRange(games);
                }

                await _context.SaveChangesAsync(cancellationToken);
                await _repository.LoadTeamCollection(games, cancellationToken);

                return games.Select(x => _repository.MapToDto(x)).ToArray();
            }
        }
    }
}