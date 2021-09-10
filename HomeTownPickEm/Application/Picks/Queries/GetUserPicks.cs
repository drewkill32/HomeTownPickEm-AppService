using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Abstract.Interfaces;
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

            public string LeagueSlug { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<GameProjection>>
        {
            private readonly IUserAccessor _accessor;
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context, IUserAccessor accessor)
            {
                _context = context;
                _accessor = accessor;
            }

            public async Task<IEnumerable<GameProjection>> Handle(Query request, CancellationToken cancellationToken)
            {
                var userId = (await _accessor.GetCurrentUserAsync()).Id;

                var dataTask = GetReleatedData(request, cancellationToken);

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
                                Picks = x.Picks.Where(p => p.UserId == userId && p.League.Slug == request.LeagueSlug)
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

                var (leagueTeamIds, cutoffDate) = await dataTask;

                foreach (var game in games)
                {
                    game.CutoffDate = cutoffDate;
                    game.Head2Head = leagueTeamIds.Contains(game.Home.Id) && leagueTeamIds.Contains(game.Away.Id);
                }


                var orderedGames = games
                    .OrderBy(x => x.StartDate)
                    .ThenBy(x => x.Home.School)
                    .ThenBy(x => x.Home.Mascot)
                    .ToArray();
                return orderedGames;
            }

            private async Task<(int[] TeamIds, DateTimeOffset? CutoffDate)> GetReleatedData(Query request,
                CancellationToken cancellationToken)
            {
                var leagueTeamIds = await _context.Teams.Where(x => x.Leagues.Any(l => l.Slug == request.LeagueSlug))
                    .Select(x => x.Id)
                    .ToArrayAsync(cancellationToken);

                var cutoffDate =
                    await _context.Calendar.Where(x => x.League.Slug == request.LeagueSlug && x.Week == request.Week)
                        .Select(x => x.CutoffDate)
                        .SingleAsync(cancellationToken);


                return (leagueTeamIds, cutoffDate);
            }
        }
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

        public string Name => $"{School} {Mascot}";
    }
}