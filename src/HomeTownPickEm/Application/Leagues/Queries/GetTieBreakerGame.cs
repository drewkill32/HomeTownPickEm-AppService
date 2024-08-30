using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Application.Games;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Queries;

public class GetTieBreakerGame
{
    public class Query : IRequest<TieBreakerGame>
    {
        public int SeasonId { get; set; }

        public int Week { get; set; }
    }


    public class Handler : IRequestHandler<Query, TieBreakerGame>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public Handler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TieBreakerGame> Handle(Query request, CancellationToken cancellationToken)
        {
            var gow = await _context.WeeklyGames.FirstOrDefaultAsync(
                x => x.Week == request.Week && x.SeasonId == request.SeasonId,
                cancellationToken);

            if (gow is not null)
            {
                var game = await _context.Games.Where(x => x.Id == gow.GameId)
                    .ProjectTo<TieBreakerGame>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                game.WeeklyGameId = gow.Id;
                return game;
            }

            throw new NotFoundException($"There is no Game of the week for week {request.Week}");
        }
    }
}