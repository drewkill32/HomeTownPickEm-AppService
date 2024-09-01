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
            var users = await _context.Season.Where(x => x.Id == request.SeasonId)
                .SelectMany(x => x.Members).ToArrayAsync(cancellationToken);

            var leaderboard = await _context.WeeklyLeaderboard
                .Where(x => x.SeasonId == request.SeasonId && x.Week == request.Week)
                .ToDictionaryAsync(x => x.UserId, y => y.TotalPoints, cancellationToken);

            //see if the max number of points is a tie


            var tieBreakerGame = await _context.WeeklyGames
                .Where(x => x.SeasonId == request.SeasonId && x.Week == request.Week)
                .Include(x => x.WeeklyGamePicks)
                .FirstOrDefaultAsync(cancellationToken);

            if (tieBreakerGame is null)
            {
                throw new NotFoundException($"There is no tie breaker game for week '{request.Week}'");
            }

            var tiebreakerDict = tieBreakerGame
                .WeeklyGamePicks.ToDictionary(x => x.UserId, y => y.TotalPoints);

            var totalPointsScored = await GetTotalPointsScored(tieBreakerGame, cancellationToken);

            var sortedUsers = users.Select(x =>
                {
                    var predictedPoints = tiebreakerDict.GetValueOrDefault(x.Id, 0);
                    var totalPoints = leaderboard.GetValueOrDefault(x.Id, 0);
                    return new
                    {
                        x.Id,
                        User = x.Name.Full,
                        Img = x.ProfileImg,
                        TotalPoints = totalPoints,
                        Tiebreaker = totalPointsScored == -1
                            ? null
                            : new
                            {
                                Predicted = predictedPoints,
                                Diff = predictedPoints - totalPointsScored,
                                AbsDiff = Math.Abs(predictedPoints - totalPointsScored)
                            }
                    };
                }).OrderByDescending(x => x.TotalPoints)
                .ThenBy(x => x.Tiebreaker?.AbsDiff)
                .ThenBy(x => x.Tiebreaker?.Diff)
                .ThenBy(x => x.User);


            return sortedUsers;
        }

        private async Task<int> GetTotalPointsScored(WeeklyGame tieBreakerGame, CancellationToken cancellationToken)
        {
            var game = await _context.Games
                .Where(x => x.Id == tieBreakerGame.GameId)
                .Select(x => new { x.HomePoints, x.AwayPoints })
                .FirstOrDefaultAsync(cancellationToken);


            //the game has finished playing
            if (game.HomePoints.HasValue && game.AwayPoints.HasValue)
            {
                return game.HomePoints.Value + game.AwayPoints.Value;
            }

            return -1;
        }
    }
}