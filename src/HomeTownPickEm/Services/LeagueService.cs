using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Services
{
    public interface ILeagueServiceFactory
    {
        ILeagueService Create(int seasonId);
    }

    public class LeagueServiceFactory : ILeagueServiceFactory
    {
        private readonly IServiceProvider _provider;

        public LeagueServiceFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ILeagueService Create(int seasonId)
        {
            return ActivatorUtilities.CreateInstance<LeagueService>(_provider, seasonId);
        }
    }

    public interface ILeagueService
    {
        Task<Team> AddTeamAsync(int teamId, CancellationToken cancellationToken);
        Task<ApplicationUser> AddUserAsync(string userId, CancellationToken cancellationToken);
        Task Update(CancellationToken cancellationToken);
    }

    public class LeagueService : ILeagueService
    {
        private readonly ApplicationDbContext _context;
        private readonly int _seasonId;
        private GameProjection[] _games;
        private Season _season;

        public LeagueService(int seasonId, ApplicationDbContext context)
        {
            _seasonId = seasonId;
            _context = context;
        }


        public async Task<ApplicationUser> AddUserAsync(string userId, CancellationToken cancellationToken)
        {
            var user = (await _context.Users
                    .AsTracking()
                    .SingleOrDefaultAsync(x => x.Id == userId, cancellationToken))
                .GuardAgainstNotFound(userId);
            var season = await GetSeason(cancellationToken);

            if (season.Members.Any(x => x.Id == userId))
            {
                return user;
            }

            season.Members.Add(user);
            if (user.TeamId.HasValue)
            {
                if (season.Teams.All(x => x.Id != user.TeamId))
                {
                    var team = await _context.Teams
                        .AsTracking().SingleAsync(x => x.Id == user.TeamId, cancellationToken);
                    season.Teams.Add(team);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            await UpdatePicks(cancellationToken);
            return user;
        }


        public async Task Update(CancellationToken cancellationToken)
        {
            await UpdatePicks(cancellationToken);
        }

        public async Task<Team> AddTeamAsync(int teamId, CancellationToken cancellationToken)
        {
            var team = (await _context.Teams
                    .AsTracking()
                    .SingleOrDefaultAsync(x => x.Id == teamId, cancellationToken))
                .GuardAgainstNotFound(teamId);
            var season = await GetSeason(cancellationToken);

            if (season.Teams.Any(x => x.Id == teamId))
            {
                return team;
            }

            season.Teams.Add(team);

            await _context.SaveChangesAsync(cancellationToken);
            await UpdatePicks(cancellationToken);
            return team;
        }

        public async Task<IEnumerable<Pick>> GetNewPicks(CancellationToken cancellationToken)
        {
            var season = await GetSeason(cancellationToken);
            var teams = season.Teams;
            var games = await GetGames(cancellationToken);

            return (from team in teams
                    from game in games.Where(x => x.ContainsTeam(team.Id))
                    select new Pick { SeasonId = season.Id, Points = 0, GameId = game.Id })
                .ToList();
        }

        private async ValueTask<GameProjection[]> GetGames(CancellationToken cancellationToken)
        {
            if (_games != null)
            {
                return _games;
            }

            var season = await GetSeason(cancellationToken);
            var teamIds = season.Teams.Select(x => x.Id).ToArray();
            _games = await _context.Games.Where(x => teamIds.Contains(x.HomeId) || teamIds.Contains(x.AwayId))
                .Select(x => new GameProjection
                {
                    Id = x.Id,
                    HomeId = x.HomeId,
                    AwayId = x.AwayId
                })
                .ToArrayAsync(cancellationToken);
            return _games;
        }


        private async ValueTask<Season> GetSeason(CancellationToken cancellationToken)
        {
            if (_season != null)
            {
                return _season;
            }

            _season = await _context.Season.Where(x => x.Id == _seasonId)
                .Include(x => x.Teams)
                .Include(x => x.Members)
                .Include(x => x.Picks)
                .AsTracking()
                .AsSplitQuery()
                .SingleOrDefaultAsync(cancellationToken);
            return _season;
        }

        private async Task<bool> IsHead2Head(int gameId, CancellationToken cancellationToken)
        {
            var games = await GetGames(cancellationToken);
            var teamIds = (await GetSeason(cancellationToken)).Teams.Select(x => x.Id).ToArray();
            var game = games.Single(g => g.Id == gameId);
            return teamIds.Contains(game.AwayId) && teamIds.Contains(game.HomeId);
        }

        private async Task RemovePicks(Team team, CancellationToken cancellationToken)
        {
            var games = await GetGames(cancellationToken);
            var season = await GetSeason(cancellationToken);

            var gamesToRemove = games.Where(x => x.ContainsTeam(team.Id)).ToArray();

            var gameIds = gamesToRemove.Select(x => x.Id).ToArray();
            var picksToRemove = await _context.Pick.Where(x => x.SeasonId == _seasonId && gameIds.Contains(x.GameId))
                .ToArrayAsync(cancellationToken);
            _context.Pick.RemoveRange(picksToRemove);

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task RemovePicks(ApplicationUser user, CancellationToken cancellationToken)
        {
            var picksToRemove = await _context.Pick.Where(x => x.SeasonId == _seasonId && x.UserId == user.Id)
                .ToArrayAsync(cancellationToken);

            _context.Pick.RemoveRange(picksToRemove);

            await _context.SaveChangesAsync(cancellationToken);
        }


        private async Task UpdatePicks(CancellationToken cancellationToken)
        {
            var season = await GetSeason(cancellationToken);

            foreach (var member in season.Members)
            {
                var newPicks = (await GetNewPicks(cancellationToken)).ToArray();
                foreach (var newPick in newPicks)
                {
                    var picks = season.Picks
                        .Where(x => x.UserId == member.Id && x.GameId == newPick.GameId)
                        .ToArray();

                    var isHead2Head = await IsHead2Head(newPick.GameId, cancellationToken);

                    if (isHead2Head && picks.Length != 2)
                    {
                        newPick.UserId = member.Id;
                        season.Picks.Add(newPick);
                    }
                    else if (picks.Length == 0)
                    {
                        newPick.UserId = member.Id;
                        season.Picks.Add(newPick);
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private struct GameProjection
        {
            public int Id { get; set; }

            public int HomeId { get; set; }

            public int AwayId { get; set; }

            public bool ContainsTeam(int teamId)
            {
                return teamId == HomeId || teamId == AwayId;
            }
        }
    }
}