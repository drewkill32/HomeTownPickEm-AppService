using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Queries
{
    public class GetMemberPicksByGame
    {
        public class Query : IRequest<IEnumerable<UserPickResponse>>
        {
            public int GameId { get; set; }

            public int LeagueId { get; set; }

            public string Season { get; set; }
        }

        public class QueryCommand : IRequestHandler<Query, IEnumerable<UserPickResponse>>
        {
            private readonly ApplicationDbContext _context;
            private readonly ISystemDate _systemDate;

            public QueryCommand(ApplicationDbContext context, ISystemDate systemDate)
            {
                _context = context;
                _systemDate = systemDate;
            }

            public async Task<IEnumerable<UserPickResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var game = await _context.Games.FirstOrDefaultAsync(x => x.Id == request.GameId, cancellationToken);

                if (game.StartDate > _systemDate.UtcNow)
                {
                    throw new UnauthorizedAccessException("You can not access the picks before the start of the game");
                }
                var users = await (from l in _context.Leaderboard
                        join t in _context.Teams on l.TeamId equals t.Id
                        join u in _context.Users on l.UserId equals u.Id
                        join p in _context.Pick on l.UserId equals p.UserId
                        where l.LeagueId == request.LeagueId
                              && l.Year == request.Season
                              && p.GameId == request.GameId
                        select new
                        {
                            u.Id,
                            u.Name,
                            t.Color,
                            t.AltColor,
                            l.LeagueId,
                            l.TotalPoints,
                            p.SelectedTeamId
                        })
                    .Distinct()
                    .OrderByDescending(x => x.TotalPoints)
                    .ThenBy(x => x.Name.First)
                    .ToArrayAsync(cancellationToken);

                var maxPoints = users.Max(x => x.TotalPoints);
                var ranks = users
                    .Select(x => x.TotalPoints)
                    .Distinct().OrderByDescending(x => x).ToList();

                var response =
                    users
                        .Select((u) =>
                            new UserPickResponse
                            {
                                UserId = u.Id,
                                Name = u.Name.Full,
                                Initials = u.Name.Initials,
                                TeamColor = u.Color,
                                TeamAltColor = u.AltColor,
                                SelectedTeamId = u.SelectedTeamId,
                                Rank = ranks.IndexOf(u.TotalPoints) + 1,
                                PointOffset = maxPoints - u.TotalPoints
                            })
                        .ToArray();
                return response;
            }
        }
    }

    public class UserPickResponse
    {
        public string Name { get; set; }

        public int Rank { get; set; }

        public string Initials { get; set; }

        public int PointOffset { get; set; }

        public string UserId { get; set; }

        public string TeamColor { get; set; }

        public string TeamAltColor { get; set; }

        public int? SelectedTeamId { get; set; }
    }
}