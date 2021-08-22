using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HomeTownPickEm.Services
{
    public interface ILeagueServiceFactory
    {
        ILeagueService Create(int leagueId);
    }

    public class LeagueServiceFactory : ILeagueServiceFactory
    {
        private readonly IServiceProvider _provider;

        public LeagueServiceFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ILeagueService Create(int leagueId)
        {
            return ActivatorUtilities.CreateInstance<LeagueService>(_provider, leagueId);
        }
    }

    public interface ILeagueService
    {
        Task<Team> AddTeamAsync(int teamId, CancellationToken cancellationToken);
        Task<ApplicationUser> AddUserAsync(string userId, CancellationToken cancellationToken);
        Task RemoveTeam(int teamId, CancellationToken cancellationToken);
        Task RemoveUser(string userId, CancellationToken cancellationToken);
    }

    public class LeagueService : ILeagueService
    {
        private readonly ApplicationDbContext _context;
        private readonly int _leagueId;
        private GameProjection[] _games;
        private League _league;

        public LeagueService(int leagueId, ApplicationDbContext context)
        {
            _leagueId = leagueId;
            _context = context;
        }


        public async Task<ApplicationUser> AddUserAsync(string userId, CancellationToken cancellationToken)
        {
            var user = (await _context.Users
                    .AsTracking()
                    .SingleOrDefaultAsync(x => x.Id == userId, cancellationToken))
                .GuardAgainstNotFound(userId);
            var league = await GetLeague(cancellationToken);
            league.Members.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            await AddPicks(user, cancellationToken);
            return user;
        }

        public async Task RemoveUser(string userId, CancellationToken cancellationToken)
        {
            var user = (await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken))
                .GuardAgainstNotFound(userId);
            var league = await GetLeague(cancellationToken);
            var member = league.Members.SingleOrDefault(x => x.Id == userId);
            if (member != null)
            {
                league.Members.Remove(member);
            }

            await _context.SaveChangesAsync(cancellationToken);
            await RemovePicks(user, cancellationToken);
        }

        public async Task<Team> AddTeamAsync(int teamId, CancellationToken cancellationToken)
        {
            var team = (await _context.Teams
                    .AsTracking()
                    .SingleOrDefaultAsync(x => x.Id == teamId, cancellationToken))
                .GuardAgainstNotFound(teamId);
            var league = await GetLeague(cancellationToken);
            league.Teams.Add(team);
            await _context.SaveChangesAsync(cancellationToken);
            await AddPicks(team, cancellationToken);
            return team;
        }


        public async Task RemoveTeam(int teamId, CancellationToken cancellationToken)
        {
            var team = (await _context.Teams.SingleOrDefaultAsync(x => x.Id == teamId, cancellationToken))
                .GuardAgainstNotFound(teamId);
            var league = await GetLeague(cancellationToken);

            var teamToRemove = league.Teams.SingleOrDefault(x => x.Id == teamId);
            if (teamToRemove != null)
            {
                league.Teams.Remove(teamToRemove);
            }

            await _context.SaveChangesAsync(cancellationToken);
            await RemovePicks(team, cancellationToken);
        }

        private async Task AddPicks(Team team, CancellationToken cancellationToken)
        {
            var games = await GetGames(team, cancellationToken);
            var gamesToAdd = games.Where(x => x.ContainsTeam(team.Id)).ToArray();
            var league = await GetLeague(cancellationToken);

            foreach (var member in league.Members)
            {
                foreach (var game in gamesToAdd)
                {
                    var pick = new Pick
                    {
                        LeagueId = league.Id,
                        Points = 0,
                        UserId = member.Id,
                        GameId = game.Id
                    };
                    await _context.Pick.AddAsync(pick, cancellationToken);
                    league.Picks.Add(pick);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddPicks(ApplicationUser user, CancellationToken cancellationToken)
        {
            var gamesToAdd = await GetGames(cancellationToken);
            var league = await GetLeague(cancellationToken);

            foreach (var game in gamesToAdd)
            {
                var pick = new Pick
                {
                    LeagueId = league.Id,
                    Points = 0,
                    UserId = user.Id,
                    GameId = game.Id
                };
                await _context.Pick.AddAsync(pick, cancellationToken);
                league.Picks.Add(pick);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async ValueTask<GameProjection[]> GetGames(CancellationToken cancellationToken)
        {
            if (_games != null)
            {
                return _games;
            }

            var league = await GetLeague(cancellationToken);
            var teamIds = league.Teams.Select(x => x.Id).ToArray();
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

        private async ValueTask<GameProjection[]> GetGames(Team team, CancellationToken cancellationToken)
        {
            var teamIds = new[] { team.Id };
            return await _context.Games.Where(x => teamIds.Contains(x.HomeId) || teamIds.Contains(x.AwayId))
                .Select(x => new GameProjection
                {
                    Id = x.Id,
                    HomeId = x.HomeId,
                    AwayId = x.AwayId
                })
                .ToArrayAsync(cancellationToken);
        }

        private async ValueTask<League> GetLeague(CancellationToken cancellationToken)
        {
            if (_league != null)
            {
                return _league;
            }

            _league = await _context.League.Where(x => x.Id == _leagueId)
                .Include(x => x.Teams)
                .Include(x => x.Members)
                .Include(x => x.Picks)
                .AsTracking()
                .AsSplitQuery()
                .SingleOrDefaultAsync(cancellationToken);
            return _league;
        }

        private async Task RemovePicks(Team team, CancellationToken cancellationToken)
        {
            var games = await GetGames(cancellationToken);
            var league = await GetLeague(cancellationToken);

            var gamesToRemove = games.Where(x => x.ContainsTeam(team.Id)).ToArray();

            var gameIds = gamesToRemove.Select(x => x.Id).ToArray();
            var picksToRemove = await _context.Pick.Where(x => x.LeagueId == _leagueId && gameIds.Contains(x.GameId))
                .ToArrayAsync(cancellationToken);
            _context.Pick.RemoveRange(picksToRemove);

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task RemovePicks(ApplicationUser user, CancellationToken cancellationToken)
        {
            var picksToRemove = await _context.Pick.Where(x => x.LeagueId == _leagueId && x.UserId == user.Id)
                .ToArrayAsync(cancellationToken);

            _context.Pick.RemoveRange(picksToRemove);

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