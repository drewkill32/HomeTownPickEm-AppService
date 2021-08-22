using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands
{
    public class UpdateLeague
    {
        public class Command : IRequest<LeagueDto>
        {
            public string Name { get; set; }
            public string Season { get; set; }
            public int[] TeamIds { get; set; } = Array.Empty<int>();
            public string[] MemberIds { get; set; } = Array.Empty<string>();
            public bool KeepExisting { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, LeagueDto>
        {
            private readonly ApplicationDbContext _context;

            public CommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<LeagueDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var league =
                    await _context.League.Where(x => x.Name == request.Name && x.Season == request.Season)
                        .Include(x => x.Teams)
                        .Include(x => x.Members)
                        .Include(x => x.Picks)
                        .AsTracking()
                        .AsSplitQuery()
                        .SingleOrDefaultAsync(cancellationToken);

                var members = await GetMembers(request, league, cancellationToken);
                var teams = await GetTeams(request, league, cancellationToken);


                if (league == null)
                {
                    throw new NotFoundException(
                        $"No League found with name {request.Name} and Season {request.Season}");
                }

                await UpdateMembers(members, league);

                await UpdateTeams(teams, league);


                _context.League.Update(league);

                await _context.SaveChangesAsync(cancellationToken);

                return league.ToLeagueDto();
            }

            private async Task AddPicks(League league, IEnumerable<Team> teams, MinGame[] gameIds)
            {
                var gamesToAdd = gameIds.Where(x => teams.Any(team => x.ContainsTeam(team.Id))).ToArray();

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
                        await _context.Pick.AddAsync(pick);
                        league.Picks.Add(pick);
                    }
                }
            }


            private async Task<ApplicationUser[]> GetMembers(Command request, League league,
                CancellationToken cancellationToken)
            {
                var users = request.KeepExisting
                    ? league.Members.ToDictionary(x => x.Id, x => x)
                    : new Dictionary<string, ApplicationUser>();

                var usersToAdd = await _context.Users.Where(x => request.MemberIds.Contains(x.Id))
                    .AsTracking()
                    .ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

                var notFoundMembers = request.MemberIds.Except(usersToAdd.Select(x => x.Key)).ToArray();
                if (notFoundMembers.Any())
                {
                    throw new NotFoundException(
                        $"member(s) not found with Id(s): '{string.Join(", ", notFoundMembers)}'");
                }

                foreach (var userEntry in usersToAdd)
                {
                    users.TryAdd(userEntry.Key, userEntry.Value);
                }

                return users.Select(x => x.Value).ToArray();
            }

            private async Task<MinGame[]> GetMinGames(int[] teamIds)
            {
                var gameIds = await _context.Games.Where(x => teamIds.Contains(x.HomeId) || teamIds.Contains(x.AwayId))
                    .Select(x => new MinGame
                    {
                        Id = x.Id,
                        HomeId = x.HomeId,
                        AwayId = x.AwayId
                    })
                    .ToArrayAsync();
                return gameIds;
            }

            private async Task<Team[]> GetTeams(Command request, League league, CancellationToken cancellationToken)
            {
                var teams = request.KeepExisting
                    ? league.Teams.ToDictionary(x => x.Id, x => x)
                    : new Dictionary<int, Team>();


                var teamsToAdd = await _context.Teams.Where(x => request.TeamIds.Contains(x.Id))
                    .AsTracking()
                    .ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

                var notFoundTeams = request.TeamIds.Except(teams.Select(x => x.Key)).ToArray();
                if (notFoundTeams.Any())
                {
                    throw new NotFoundException(
                        $"team(s) not found with Id(s): '{string.Join(", ", notFoundTeams.Select(x => x.ToString()))}'");
                }

                foreach (var teamEntry in teamsToAdd)
                {
                    teams.TryAdd(teamEntry.Key, teamEntry.Value);
                }

                return teams.Select(x => x.Value).ToArray();
            }

            private static void RemovePicks(League league, Team team, MinGame[] gameIds)
            {
                var gamesToRemove = gameIds.Where(x => x.ContainsTeam(team.Id)).Select(x => x.Id)
                    .ToArray();
                var picksToRemove = league.Picks.Where(x => gamesToRemove.Contains(x.GameId)).ToArray();
                foreach (var pick in picksToRemove)
                {
                    league.Picks.Remove(pick);
                }
            }

            private async Task UpdateMembers(ApplicationUser[] members, League league)
            {
                var newMembers = members.Except(league.Members, ModelEquality<ApplicationUser>.IdComparer).ToArray();
                var oldMembers = league.Members.Except(members, ModelEquality<ApplicationUser>.IdComparer).ToArray();
                foreach (var member in newMembers)
                {
                    league.Members.Add(member);
                    var gameIds = await GetMinGames(league.Teams.Select(x => x.Id).ToArray());
                    await AddPicks(league, league.Teams, gameIds);
                }

                foreach (var member in oldMembers)
                {
                    league.Members.Remove(member);
                    var picksToRemove = league.Picks.Where(x => x.UserId == member.Id).ToArray();
                    foreach (var pick in picksToRemove)
                    {
                        league.Picks.Remove(pick);
                    }
                }
            }

            private async Task UpdateTeams(Team[] teams, League league)
            {
                var newTeams = teams.Except(league.Teams, ModelEquality<Team>.IdComparer).ToArray();
                var oldTeams = league.Teams.Except(teams, ModelEquality<Team>.IdComparer).ToArray();
                var teamIds = newTeams.Select(x => x.Id).Concat(oldTeams.Select(x => x.Id)).ToArray();

                var gameIds = await GetMinGames(teamIds);

                foreach (var team in newTeams)
                {
                    league.Teams.Add(team);

                    await AddPicks(league, new[] { team }, gameIds);
                }

                foreach (var team in oldTeams)
                {
                    league.Teams.Remove(team);
                    RemovePicks(league, team, gameIds);
                }
            }

            private struct MinGame
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
}