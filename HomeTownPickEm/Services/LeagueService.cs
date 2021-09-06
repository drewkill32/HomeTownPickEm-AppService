using System;
using System.Collections.Generic;
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
        Task Update(CancellationToken cancellationToken);
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

            if (league.Members.Any(x => x.Id == userId))
            {
                return user;
            }

            league.Members.Add(user);
            if (user.TeamId.HasValue)
            {
                if (league.Teams.All(x => x.Id != user.TeamId))
                {
                    var team = await _context.Teams
                        .AsTracking().SingleAsync(x => x.Id == user.TeamId, cancellationToken);
                    league.Teams.Add(team);
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
            var league = await GetLeague(cancellationToken);

            if (league.Teams.Any(x => x.Id == teamId))
            {
                return team;
            }

            league.Teams.Add(team);

            await _context.SaveChangesAsync(cancellationToken);
            await UpdatePicks(cancellationToken);
            return team;
        }

        public async Task<IEnumerable<Pick>> GetNewPicks(CancellationToken cancellationToken)
        {
            var league = await GetLeague(cancellationToken);
            var teams = league.Teams;
            var games = await GetGames(cancellationToken);

            return (from team in teams
                    from game in games.Where(x => x.ContainsTeam(team.Id))
                    select new Pick { LeagueId = league.Id, Points = 0, GameId = game.Id })
                .ToList();
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

        private async Task<bool> IsHead2Head(int gameId, CancellationToken cancellationToken)
        {
            var games = await GetGames(cancellationToken);
            var teamIds = (await GetLeague(cancellationToken)).Teams.Select(x => x.Id).ToArray();
            var game = games.Single(g => g.Id == gameId);
            return teamIds.Contains(game.AwayId) && teamIds.Contains(game.HomeId);
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


        private async Task UpdatePicks(CancellationToken cancellationToken)
        {
            var league = await GetLeague(cancellationToken);

            foreach (var member in league.Members)
            {
                var newPicks = (await GetNewPicks(cancellationToken)).ToArray();
                foreach (var newPick in newPicks)
                {
                    var picks = league.Picks
                        .Where(x => x.UserId == member.Id && x.GameId == newPick.GameId)
                        .ToArray();

                    var isHead2Head = await IsHead2Head(newPick.GameId, cancellationToken);

                    if (isHead2Head && picks.Length != 2)
                    {
                        newPick.UserId = member.Id;
                        league.Picks.Add(newPick);
                    }
                    else if (picks.Length == 0)
                    {
                        newPick.UserId = member.Id;
                        league.Picks.Add(newPick);
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