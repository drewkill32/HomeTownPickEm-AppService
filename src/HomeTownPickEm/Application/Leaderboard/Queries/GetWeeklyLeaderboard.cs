using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using HomeTownPickEm.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leaderboard.Queries;

public class GetWeeklyLeaderboard
{
    public class Query : IRequest<object>
    {
        public int SeasonId { get; set; }

        public int Week { get; set; }
    }

    public class Handler : IRequestHandler<Query, object>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> Handle(Query request, CancellationToken cancellationToken)
        {
            var leaderboard = await _context.WeeklyLeaderboard
                .Where(x => x.SeasonId == request.SeasonId && x.Week == request.Week)
                .ToArrayAsync(cancellationToken);

            //see if the max number of points is a tie


            var tieBreakerGame = await _context.WeeklyGames
                .Where(x => x.SeasonId == request.SeasonId && x.Week == request.Week)
                .Include(x => x.WeeklyGamePicks)
                .FirstOrDefaultAsync(cancellationToken);

            if (tieBreakerGame is null)
            {
                throw new NotFoundException($"There is no tie breaker game for week '{request.Week}'");
            }

            var userLookup = tieBreakerGame
                .WeeklyGamePicks.ToDictionary(x => x.UserId, y => y.TotalPoints);

            var totalPointsScored = await GetTotalPointsScored(tieBreakerGame, cancellationToken);

            var sortedLeaderboard = leaderboard
                .Select(x =>
                {
                    var predictedPoints = userLookup.GetValueOrDefault(x.UserId, 0);
                    return new
                    {
                        x.UserId,
                        x.UserFirstName,
                        x.UserLastName,
                        x.TotalPoints,
                        TeamLogo = LogoHelper.GetSingleLogo(x.TeamLogos),
                        Diff = predictedPoints - totalPointsScored,
                        AbsoluteDiff = Math.Abs(predictedPoints - totalPointsScored)
                    };
                }).OrderByDescending(x => x.TotalPoints)
                .ThenBy(x => x.AbsoluteDiff)
                .ThenBy(x => x.UserFirstName)
                .ThenBy(x => x.UserLastName);


            return sortedLeaderboard;
        }

        private async Task<int> GetTotalPointsScored(WeeklyGame tieBreakerGame, CancellationToken cancellationToken)
        {
            var game = await _context.Games
                .Where(x => x.Id == tieBreakerGame.GameId)
                .Select(x => new
                {
                    x.HomePoints,
                    x.AwayPoints
                })
                .FirstOrDefaultAsync(cancellationToken);

            //the game has finished playing
            var totalPointsScored = 0;
            if (game.HomePoints.HasValue && game.AwayPoints.HasValue)
            {
                totalPointsScored = game.HomePoints.Value + game.AwayPoints.Value;
            }

            return totalPointsScored;
        }
    }
}