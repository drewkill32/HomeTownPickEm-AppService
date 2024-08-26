using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands;

public class NewGameOfTheWeek
{
    public class Command : ILeagueCommissionerRequest<int>
    {
        public int LeagueId { get; set; }

        public int SeasonId { get; set; }

        public int Week { get; set; }
    }


    public class Handler : IRequestHandler<Command, int>
    {
        private readonly ISystemDate _systemDate;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NewGameOfTheWeek> _logger;
        private readonly Random _random;
        private readonly SemaphoreSlim _mutex = new(1);


        public Handler(ISystemDate systemDate, ApplicationDbContext context, ILogger<NewGameOfTheWeek> logger)
        {
            _systemDate = systemDate;
            _context = context;
            _logger = logger;
            _random = new();
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            try
            {
                await _mutex.WaitAsync(cancellationToken);
                //check if the gow exists
                var gow = await _context.WeeklyGames.FirstOrDefaultAsync(
                    x => x.Week == request.Week && x.SeasonId == request.SeasonId,
                    cancellationToken);

                if (gow is not null)
                {
                    return gow.GameId;
                }

                var teamIds = await _context.Season.Where(x => x.Id == request.SeasonId)
                    .SelectMany(s => s.Teams.Select(t => t.Id)).ToArrayAsync(cancellationToken);

                //get the games that are in the league
                var games = await _context.Games.WhereTeamsArePlaying(teamIds)
                    .Where(x => x.Week == request.Week && x.StartTimeTbd == false && x.StartDate > _systemDate.UtcNow)
                    .Select(x => x.Id)
                    .ToArrayAsync(cancellationToken);

                var randomGame = _random.Next(0, games.Length);

                var gameId = games[randomGame];
                _context.WeeklyGames.Add(new()
                {
                    GameId = gameId,
                    SeasonId = request.SeasonId,
                    Week = request.Week
                });
                await _context.SaveChangesAsync(cancellationToken);
                return gameId;
            }
            finally
            {
                _mutex.Release();
            }
        }
    }
}