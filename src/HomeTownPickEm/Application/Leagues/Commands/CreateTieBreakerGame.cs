using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Application.Games;
using HomeTownPickEm.Application.Teams;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands;

public class CreateTieBreakerGame
{
    public class Command : IRequest<GameDto>
    {
        public int SeasonId { get; set; }

        public int Week { get; set; }
    }


    public class Handler : IRequestHandler<Command, GameDto>
    {
        private readonly ISystemDate _systemDate;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly Random _random;
        private readonly SemaphoreSlim _mutex = new(1);


        public Handler(ISystemDate systemDate, ApplicationDbContext context, IMapper mapper)
        {
            _systemDate = systemDate;
            _context = context;
            _mapper = mapper;
            _random = new();
        }

        public async Task<GameDto> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                var gow = await _context.WeeklyGames.FirstOrDefaultAsync(
                    x => x.Week == request.Week && x.SeasonId == request.SeasonId,
                    cancellationToken);

                if (gow is not null)
                {
                    return await _context.Games.Where(x => x.Id == gow.GameId)
                        .ProjectTo<GameDto>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(cancellationToken);
                }

                //make sure to one create one
                await _mutex.WaitAsync(cancellationToken);

                var season = await _context.Season.Where(x => x.Id == request.SeasonId)
                    .Select(s => new
                    {
                        s.Id,
                        s.Year,
                        TeamIds = s.Teams.Select(t => t.Id)
                    }).FirstOrDefaultAsync(cancellationToken);

                var teamIds = season.TeamIds.ToArray();
                //get the games that are in the league
                var games = await _context.Games.WhereTeamsArePlaying(teamIds)
                    .Where(x => x.Season == season.Year && x.SeasonType == "regular" && x.Week == request.Week &&
                                x.StartTimeTbd == false &&
                                x.StartDate > _systemDate.UtcNow)
                    .ProjectTo<GameDto>(_mapper.ConfigurationProvider)
                    .ToArrayAsync(cancellationToken);

                if (games.Length == 0)
                {
                    throw new NotFoundException("There are not games to select");
                }

                var randomGame = _random.Next(0, games.Length);


                var selectedGame = games[randomGame];
                _context.WeeklyGames.Add(new()
                {
                    GameId = selectedGame.Id,
                    SeasonId = request.SeasonId,
                    Week = request.Week
                });
                await _context.SaveChangesAsync(cancellationToken);
                return games[randomGame];
            }
            finally
            {
                _mutex.Release();
            }
        }
    }
}