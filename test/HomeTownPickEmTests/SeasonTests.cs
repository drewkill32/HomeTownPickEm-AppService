using System;
using System.Linq;
using FluentAssertions;
using HomeTownPickEm.Data.Extensions;
using HomeTownPickEm.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HomeTownPickEm;

public class SeasonTests
{
    public int MichTeamId => 130; // Michigan Wolverines
    public int MSUTeamId => 127; // Michigan State Spartans
    public DatabaseFixture Database { get; } = new();


    [Fact]
    public void AddMemberShould_AddMemberToLeagueWithPicks()
    {
        var userId = Database.CreateUser().Id;

        int seasonId;
        var leagueId = Database.CreateLeague().Id;
        var gameIds = Array.Empty<int>();
        using (var context = Database.CreateTrackingDbContext())
        {
            var season = new Season
            {
                Year = "2022",
                LeagueId = leagueId
            };
            //we need to fetch the entity from the database to get the id
            var user = context.Users.FirstOrDefault(u => u.Id == userId);
            var team = context.Teams.FirstOrDefault(x => x.Id == MichTeamId);
            var games = context.Games.WhereTeamIsPlaying(MichTeamId).ToArray();
            gameIds = games.Select(x => x.Id).ToArray();
            season.AddMember(user);
            season.AddTeam(team, games);
            context.Season.Add(season);
            context.SaveChanges();
            seasonId = season.Id;
        }

        var dbSeason = GetSeasonFromDatabase(seasonId);


        dbSeason.Members.Should().ContainSingle(x => x.Id == userId);
        dbSeason.Teams.Should().ContainSingle(x => x.Id == MichTeamId);
        dbSeason.Picks.Select(x => x.GameId).Should()
            .BeEquivalentTo(gameIds);
    }

    [Fact]
    public void AddTeam_WithoutMembers_Should_AddTeamToLeague_ButNotPicks()
    {
        int seasonId;
        var leagueId = Database.CreateLeague().Id;

        using (var context = Database.CreateTrackingDbContext())
        {
            var season = new Season
            {
                Year = "2022",
                LeagueId = leagueId
            };
            var team = context.Teams.FirstOrDefault(x => x.Id == MichTeamId);
            var games = context.Games.WhereTeamIsPlaying(MichTeamId).ToArray();
            season.AddTeam(team, games);
            context.Season.Add(season);
            context.SaveChanges();
            seasonId = season.Id;
        }

        var dbSeason = GetSeasonFromDatabase(seasonId);
        dbSeason.Teams.Should().ContainSingle(x => x.Id == MichTeamId);
        dbSeason.Members.Should().BeEmpty();
        dbSeason.Picks.Should().BeEmpty();
    }

    [Fact]
    public void AddTwoUser_ShouldGetAllPicksForTwoTeam()
    {
        var teamIds = new[] { MichTeamId, MSUTeamId };

        var users = new[] { Database.CreateUser(), Database.CreateUser() };

        var h2hGameId = 401282777;

        int seasonId;
        var gameCount = 0;
        var teamCount = teamIds.Length;

        var leagueId = Database.CreateLeague().Id;


        using (var context = Database.CreateTrackingDbContext())
        {
            var season = new Season
            {
                Year = "2022",
                LeagueId = leagueId
            };

            //get from the db for change tracking
            var userId = users.Select(x => x.Id).ToArray();
            var dbUsers = context.Users.Where(x => userId.Contains(x.Id)).ToArray();
            foreach (var user in dbUsers)
            {
                season.AddMember(user);
            }

            foreach (var teamId in teamIds)
            {
                var team = context.Teams.FirstOrDefault(x => x.Id == teamId);
                var games = context.Games.WhereTeamIsPlaying(team.Id).ToArray();
                season.AddTeam(team, games);
                gameCount += games.Length;
            }

            context.Season.Add(season);
            context.SaveChanges();
            seasonId = season.Id;
        }

        var dbSeason = GetSeasonFromDatabase(seasonId);
        dbSeason.Members.Should().HaveCount(users.Length);
        dbSeason.Teams.Should().HaveCount(teamIds.Length);
        dbSeason.Picks.Should().HaveCount(gameCount * users.Length);
    }

    private Season GetSeasonFromDatabase(int seasonId)
    {
        using var ctx = Database.CreateDbContext();
        return ctx.Season.Where(x => x.Id == seasonId)
            .Include(x => x.Teams)
            .Include(x => x.Picks)
            .ThenInclude(p => p.Game)
            .Include(x => x.Members)
            .Include(x => x.League)
            .FirstOrDefault();
    }
}