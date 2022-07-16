using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using HomeTownPickEm.Services;
using HomeTownPickEm.Services.Cfbd;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
                var gamesResponse = (await _httpClient.GetGames(request, cancellationToken))
                    .ToHashSet(new IdEqualityComparer<GameResponse>());

                var games = gamesResponse.Select(x => x.ToGame())
                    .ToArray();

                var dbGames = await _context.Games.Select(x => x.Id).ToArrayAsync(cancellationToken);
                var (existingGames, newGames) = GetGameDiffs(games, dbGames);
                if (existingGames.Any())
                {
                    _context.Games.UpdateRange(existingGames);
                }

                if (newGames.Any())
                {
                    _context.Games.AddRange(newGames);
                }

                await _context.SaveChangesAsync(cancellationToken);
                await _repository.LoadTeamCollection(games, cancellationToken);

                return games.Select(x => _repository.MapToDto(x)).ToArray();
            }

            private (Game[] existingGames, Game[] newGames) GetGameDiffs(IEnumerable<Game> games, int[] dbGameIds)
            {
                var existingGames = games.Where(x => dbGameIds.Contains(x.Id)).ToArray();
                var newGames = games.Where(x => !dbGameIds.Contains(x.Id)).ToArray();
                return (existingGames, newGames);
            }
        }
    }

  
}