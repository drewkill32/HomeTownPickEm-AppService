using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Games.Commands;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using HomeTownPickEm.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Picks.Queries
{
    public class GetUserPicks
    {
        public class Query : IRequest<IEnumerable<GameProjection>>
        {
            public int Week { get; set; }

            public int LeagueId { get; set; }

            public string Season { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<GameProjection>>
        {
            private readonly IUserAccessor _accessor;
            private readonly IBackgroundWorkerQueue _workerQueue;
            private readonly ISystemDate _date;
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context, IUserAccessor accessor,
                IBackgroundWorkerQueue workerQueue,
                ISystemDate date)
            {
                _context = context;
                _accessor = accessor;
                _workerQueue = workerQueue;
                _date = date;
            }

            public async Task<IEnumerable<GameProjection>> Handle(Query request, CancellationToken cancellationToken)
            {
                var userId = (await _accessor.GetCurrentUserAsync()).Id;
                await UpdateCache(request, cancellationToken);
                var seasonId = await _context.Season
                    .Where(s => s.Year == request.Season && s.LeagueId == request.LeagueId)
                    .Select(s => s.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                var (leagueTeamIds, pickTotals) = await GetRelatedData(request, seasonId, cancellationToken);

                var games =
                    await _context.Games
                        .Where(x => x.Week == request.Week)
                        .Select(x =>
                            new GameProjection
                            {
                                Id = x.Id,
                                StartDate = x.StartDate,
                                StartTimeTbd = x.StartTimeTbd,
                                Week = x.Week,
                                SeasonType = x.SeasonType,
                                Season = x.Season,
                                WinnerId = x.WinnerId,
                                Winner = x.Winner,
                                Away = new TeamProjection
                                {
                                    Id = x.Away.Id,
                                    Points = x.AwayPoints,
                                    School = x.Away.School,
                                    Mascot = x.Away.Mascot,
                                    Logo = x.Away.Logos,
                                    Color = x.Away.Color,
                                    AltColor = x.Away.AltColor
                                },
                                Home = new TeamProjection
                                {
                                    Id = x.Home.Id,
                                    Points = x.HomePoints,
                                    School = x.Home.School,
                                    Mascot = x.Home.Mascot,
                                    Logo = x.Home.Logos,
                                    Color = x.Home.Color,
                                    AltColor = x.Home.AltColor
                                },
                                Picks = x.Picks.Where(p =>
                                        p.UserId == userId && p.SeasonId == seasonId)
                                    .Select(p => new PickProjection
                                    {
                                        Id = p.Id,
                                        GameId = p.GameId,
                                        UserId = p.UserId,
                                        SelectedTeamId = p.SelectedTeamId
                                    })
                            })
                        .Where(x => x.Picks.Any())
                        .ToArrayAsync(cancellationToken);


                foreach (var game in games)
                {
                    game.CutoffDate = game.StartDate.AddMinutes(-1);
                    game.IsPastCutoff = game.StartDate < _date.UtcNow;
                    game.CurrentDateTime = _date.UtcNow;
                    game.Head2Head = leagueTeamIds.Contains(game.Home.Id) && leagueTeamIds.Contains(game.Away.Id);
                    if (_date.UtcNow > game.CutoffDate && pickTotals.TryGetValue(game.Id, out var totals))
                    {
                        game.Home.PercentPicked = totals.HomePercent;
                        game.Away.PercentPicked = totals.AwayPercent;
                    }
                 
                }


                var orderedGames = games
                    .OrderBy(x => x.StartDate)
                    .ThenBy(x => x.Home.School)
                    .ThenBy(x => x.Home.Mascot)
                    .ToArray();
                return orderedGames;
            }

            private async Task UpdateCache(Query request, CancellationToken cancellationToken)
            {
                var seasonCache = await _context.SeasonCaches.FirstOrDefaultAsync(
                    x => x.Type == "regular" && x.Season == request.Season && x.Week == request.Week,
                    cancellationToken);
                var now = _date.UtcNow;
                if (seasonCache == null || seasonCache.LastRefresh.AddMinutes(20) < now)
                {
                    var command = new LoadGames.Command
                    {
                        SeasonType = "regular",
                        Week = request.Week,
                        Year = request.Season
                    };
                    _workerQueue.Queue(command.ToString(), command);
                    if (seasonCache == null)
                    {
                        _context.SeasonCaches.Add(new SeasonCache
                        {
                            LastRefresh = now,
                            Season = request.Season,
                            Type = "regular",
                            Week = request.Week
                        });
                    }
                    else
                    {
                        seasonCache.LastRefresh = now;
                        _context.SeasonCaches.Update(seasonCache);
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

            private async Task<(int[] TeamIds, Dictionary<int, PickTotalDto> PickTotals)>
                GetRelatedData(Query request,
                    int seasonId,
                    CancellationToken cancellationToken)
            {
                var leagueTeamIds = await _context.Teams
                    .Where(x => x.Seasons.Any(s => s.Id == seasonId))
                    .Select(x => x.Id)
                    .ToArrayAsync(cancellationToken);

                var cutoffDate =
                    await _context.Calendar.Where(x => x.Season == request.Season && x.Week == request.Week)
                        .Select(x => x.FirstGameStart)
                        .SingleAsync(cancellationToken);


                var pickTotals = _date.UtcNow >= cutoffDate
                    ? await _context.Pick.Where(x =>
                            x.Game.Week == request.Week && x.SeasonId == seasonId &&
                            x.SelectedTeamId != null)
                        .Select(x => new
                        {
                            x.GameId, x.SelectedTeamId, x.Game.AwayId, x.Game.HomeId,
                            HomeSelected = x.SelectedTeamId == x.Game.HomeId,
                            AwaySelected = x.SelectedTeamId == x.Game.AwayId
                        })
                        .GroupBy(x => x.GameId)
                        .Select(x => new PickTotalDto
                        {
                            GameId = x.Key,
                            HomePicked = x.Count(y => y.HomeSelected),
                            AwayPicked = x.Count(y => y.AwaySelected),
                            Total = x.Count()
                        })
                        .ToDictionaryAsync(dto => dto.GameId, dto => dto, cancellationToken)
                    : new Dictionary<int, PickTotalDto>();

                return (leagueTeamIds, pickTotals);
            }
        }
    }


    public class PickTotalDto
    {
        public int HomePicked { get; set; }

        public int AwayPicked { get; set; }

        public int Total { get; set; }

        public double HomePercent => Math.Round(HomePicked / (double)Total * 100, 0);

        public double AwayPercent => Math.Round(AwayPicked / (double)Total * 100, 0);
        public int GameId { get; set; }
    }

    public class GameProjection : IMapFrom<Game>
    {
        public int Id { get; set; }

        public int Week { get; set; }

        public string SeasonType { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public bool StartTimeTbd { get; set; }


        public TeamProjection Home { get; set; }

        public TeamProjection Away { get; set; }

        public IEnumerable<PickProjection> Picks { get; set; }

        public int? WinnerId { get; set; }
        public string Season { get; set; }
        public DateTimeOffset? CutoffDate { get; set; }
        public string Winner { get; set; }
        public bool Head2Head { get; set; }
        public bool IsPastCutoff { get; set; }
        public DateTimeOffset CurrentDateTime { get; set; }
    }


    public class PickProjection : IMapFrom<Pick>
    {
        public int GameId { get; set; }
        public int? SelectedTeamId { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class TeamProjection : IMapFrom<Team>
    {
        private string _logo;
        public int Id { get; set; }
        public int? Points { get; set; }

        public string Logo
        {
            get => _logo;
            set => _logo = LogoHelper.GetSingleLogo(value);
        }

        public string Color { get; set; }
        public string AltColor { get; set; }
        public string Mascot { get; set; }
        public string School { get; set; }

        public double? PercentPicked { get; set; }
        public string Name => $"{School} {Mascot}";
    }
}