using System;
using System.Linq;
using FluentAssertions;
using HomeTownPickEm.Data;
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
                Year = "2021",
                LeagueId = leagueId
            };
            //we need to fetch the entity from the database to get the id
            var user = context.Users.FirstOrDefault(u => u.Id == userId);
            var team = context.Teams.FirstOrDefault(x => x.Id == MichTeamId);
            season.AddTeam(team);
            var games = context.Games.WhereTeamIsPlaying(team).ToArray();
            gameIds = games.Select(x => x.Id).ToArray();
            season.AddMember(user, games);
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
    public void UpdatePicks_Should_GrantPoints()
    {
        var userId = Database.CreateUser().Id;
        var leagueId = Database.CreateLeague().Id;
        var seasonId = CreateSeason(leagueId, userId).Id;

        using (var context = Database.CreateTrackingDbContext())
        {
            var season = GetSeasonFromDatabase(seasonId, context);
            // pick the home team for every game
            foreach (var pick in season.Picks)
            {
                if (pick.Game.TeamIsPlaying(MichTeamId))
                {
                    pick.SelectedTeamId = MichTeamId;
                }
                else
                {
                    pick.SelectedTeamId = MSUTeamId;
                }
            }

            context.SaveChanges();
        }

        using (var context = Database.CreateTrackingDbContext())
        {
            var season = GetSeasonFromDatabase(seasonId, context);
            var games = season.Picks.Select(x => x.Game).Where(x => x.Week == 1).ToArray();
            season.UpdatePicks(games);
            context.SaveChanges();
        }

        var dbSeason = GetSeasonFromDatabase(seasonId);

        var firstWeekPicks = dbSeason.Picks.Where(p => p.Game.Week == 1).ToArray();
        //both MSU and Mich won their first games
        firstWeekPicks.Should()
            .Contain(p => p.Game.TeamIsPlaying(MichTeamId) && p.Points == 1)
            .And
            .Contain(p => p.Game.TeamIsPlaying(MSUTeamId) && p.Points == 1);
    }

    [Fact]
    public void UpdatePicks_Should_GrantTwoPoints_IfHead2Head()
    {
        var userId = Database.CreateUser().Id;
        var leagueId = Database.CreateLeague().Id;
        var seasonId = CreateSeason(leagueId, userId).Id;

        using (var context = Database.CreateTrackingDbContext())
        {
            var season = GetSeasonFromDatabase(seasonId, context);
            // pick the home team for every game

            // pick the Mich vs MSU game for 2021
            foreach (var pick in season.Picks.Where(p =>
                         p.Game.TeamIsPlaying(MichTeamId) && p.Game.TeamIsPlaying(MSUTeamId)))
            {
                pick.Game.AwayPoints = 100; //Mich won 100 - 0 😊 
                pick.Game.HomePoints = 0;

                pick.SelectedTeamId = MichTeamId;
            }

            context.SaveChanges();
        }

        using (var context = Database.CreateTrackingDbContext())
        {
            var season = GetSeasonFromDatabase(seasonId, context);
            var games = season.Picks.Select(x => x.Game)
                .Where(x => x.TeamIsPlaying(MichTeamId) && x.TeamIsPlaying(MSUTeamId)).ToArray();
            season.UpdatePicks(games);
            context.SaveChanges();
        }

        var dbSeason = GetSeasonFromDatabase(seasonId);

        var h2hPicks = dbSeason.Picks.Where(p => p.Game.TeamIsPlaying(MichTeamId) && p.Game.TeamIsPlaying(MSUTeamId))
            .ToArray();
        //all points should be 1
        h2hPicks.Should().NotContain(p => p.Points == 0);
    }

    [Fact]
    public void UpdatePicks_Should_GrantOnePoint_IfSplitHead2Head()
    {
        var userId = Database.CreateUser().Id;
        var leagueId = Database.CreateLeague().Id;
        var seasonId = CreateSeason(leagueId, userId).Id;

        using (var context = Database.CreateTrackingDbContext())
        {
            var season = GetSeasonFromDatabase(seasonId, context);
            // pick the home team for every game

            // pick the Mich vs MSU game for 2021
            var picks = season.Picks.Where(p =>
                p.Game.TeamIsPlaying(MichTeamId) && p.Game.TeamIsPlaying(MSUTeamId)).ToArray();
            for (var i = 0; i < picks.Length; i++)
            {
                var pick = picks[i];
                pick.Game.AwayPoints = 100; //Mich won 100 - 0 😊 
                pick.Game.HomePoints = 0;
                pick.SelectedTeamId = i % 2 == 0 ? MichTeamId : MSUTeamId;
            }


            context.SaveChanges();
        }

        using (var context = Database.CreateTrackingDbContext())
        {
            var season = GetSeasonFromDatabase(seasonId, context);
            var games = season.Picks.Select(x => x.Game)
                .Where(x => x.TeamIsPlaying(MichTeamId) && x.TeamIsPlaying(MSUTeamId)).ToArray();
            season.UpdatePicks(games);
            context.SaveChanges();
        }

        var dbSeason = GetSeasonFromDatabase(seasonId);

        var h2hPicks = dbSeason.Picks.Where(p => p.Game.TeamIsPlaying(MichTeamId) && p.Game.TeamIsPlaying(MSUTeamId))
            .ToArray();
        //all points should be 1
        h2hPicks.Should()
            .Contain(p => p.Points == 1)
            .And
            .Contain(p => p.Points == 0);
    }

    private Season CreateSeason(int leagueId, string userId)
    {
        using (var context = Database.CreateTrackingDbContext())
        {
            var season = new Season
            {
                Year = "2021",
                LeagueId = leagueId
            };
            //we need to fetch the entity from the database to get the id
            var user = context.Users.FirstOrDefault(u => u.Id == userId);
            var michTeam = context.Teams.FirstOrDefault(x => x.Id == MichTeamId);
            var msuTeam = context.Teams.FirstOrDefault(x => x.Id == MSUTeamId);
            season.AddTeam(michTeam);
            season.AddTeam(msuTeam);
            var games = context.Games.WhereTeamsArePlaying(new[] { michTeam, msuTeam }).ToArray();

            season.AddMember(user, games);
            context.Season.Add(season);
            context.SaveChanges();
            return season;
        }
    }

    [Fact]
    public void AddTeam_Should_Not_Add_Games_From_Another_Season()
    {
        var userId = Database.CreateUser().Id;

        int seasonId;
        var leagueId = Database.CreateLeague().Id;
        var gameIds = Array.Empty<int>();
        using (var context = Database.CreateTrackingDbContext())
        {
            var season = new Season
            {
                Year = "2021",
                LeagueId = leagueId
            };
            //we need to fetch the entity from the database to get the id
            var user = context.Users.FirstOrDefault(u => u.Id == userId);
            var team = context.Teams.FirstOrDefault(x => x.Id == MichTeamId);
            season.AddTeam(team);
            var games = context.Games.WhereTeamIsPlaying(team).ToArray();
            var prevSeasonGames = games.Select(g => new Game
            {
                Id = g.Id + 100000,
                Season = "2020",
                Week = g.Week,
                AwayId = g.AwayId,
                HomeId = g.HomeId,
                AwayPoints = g.AwayPoints,
                HomePoints = g.HomePoints,
                SeasonType = g.SeasonType,
                StartDate = g.StartDate.AddYears(-1),
                StartTimeTbd = g.StartTimeTbd
            }).ToArray();
            var allGames = games.Concat(prevSeasonGames).ToArray();
            gameIds = games.Select(x => x.Id).ToArray();
            season.AddMember(user, allGames);
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
                Year = "2021",
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
    public void AddTwoUser_ShouldGetAllPicksForTwoTeams()
    {
        var teamIds = new[] { MichTeamId, MSUTeamId };

        var users = new[] { Database.CreateUser(), Database.CreateUser() };


        int seasonId;
        var gameCount = 0;

        var leagueId = Database.CreateLeague().Id;


        using (var context = Database.CreateTrackingDbContext())
        {
            var season = new Season
            {
                Year = "2021",
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

    [Fact]
    public void Removing_Team_Should_RemoveAllGames_Except_KeepHead2HeadGame()
    {
        var teamIds = new[] { MichTeamId, MSUTeamId };

        var user = Database.CreateUser();


        var leagueId = Database.CreateLeague().Id;

        var season = CreateSeason(leagueId, user, teamIds);

        using (var context = Database.CreateTrackingDbContext())
        {
            var sut = context.Season
                .Include(x => x.Members)
                .Include(x => x.Picks)
                .ThenInclude(p => p.Game)
                .Include(x => x.Teams)
                .First(x => x.Id == season.Id);
            var team = context.Teams.First(x => x.Id == MSUTeamId);
            sut.RemoveTeam(team);
            context.SaveChanges();
        }

        var dbSeason = GetSeasonFromDatabase(season.Id);


        dbSeason.Teams.Should().NotContain(x => x.Id == MSUTeamId);
        // one of the head to head games should be kept because we know MSU is playing against Mich
        dbSeason.Picks.Should().ContainSingle(x => x.Game.TeamIsPlaying(MSUTeamId));
    }

    [Fact]
    public void Removing_Member_Should_RemoveAllPicksForMember()
    {
        var teamIds = new[] { MichTeamId, MSUTeamId };

        var users = new[] { Database.CreateUser(), Database.CreateUser() };

        var userToRemove = users[0];

        var leagueId = Database.CreateLeague().Id;

        var season = CreateSeason(leagueId, users, teamIds);

        using (var context = Database.CreateTrackingDbContext())
        {
            var sut = context.Season
                .Include(x => x.Members)
                .Include(x => x.Picks)
                .ThenInclude(p => p.Game)
                .Include(x => x.Teams)
                .First(x => x.Id == season.Id);
            var dbUser = context.Users.First(x => x.Id == userToRemove.Id);
            sut.RemoveMember(dbUser);
            context.SaveChanges();
        }

        var dbSeason = GetSeasonFromDatabase(season.Id);


        dbSeason.Members.Should().HaveCountGreaterThan(0)
            .And.NotContain(x => x.Id == userToRemove.Id);

        dbSeason.Picks.Should().HaveCountGreaterThan(0)
            .And.NotContain(x => x.UserId == userToRemove.Id);
    }

    private Season CreateSeason(int leagueId, ApplicationUser user, params int[] teamIds)
    {
        return CreateSeason(leagueId, new[] { user }, teamIds);
    }

    private Season CreateSeason(int leagueId, ApplicationUser[] users, params int[] teamIds)
    {
        using var context = Database.CreateTrackingDbContext();
        var season = new Season
        {
            Year = "2021",
            LeagueId = leagueId
        };

        var userIds = users.Select(x => x.Id).ToArray();
        //get from the db for change tracking
        var dbUsers = context.Users.Where(x => userIds.Contains(x.Id)).ToArray();
        foreach (var dbUser in dbUsers)
        {
            season.AddMember(dbUser);
        }

        foreach (var teamId in teamIds)
        {
            var team = context.Teams.FirstOrDefault(x => x.Id == teamId);
            var games = context.Games.WhereTeamIsPlaying(team.Id).ToArray();
            season.AddTeam(team, games);
        }

        context.Season.Add(season);
        context.SaveChanges();

        return season;
    }

    private Season GetSeasonFromDatabase(int seasonId, ApplicationDbContext context = null)
    {
        var disposeContext = false;
        if (context == null)
        {
            context = Database.CreateDbContext();
            disposeContext = true;
        }

        var season = context.Season.Where(x => x.Id == seasonId)
            .Include(x => x.Teams)
            .Include(x => x.Picks)
            .ThenInclude(p => p.Game)
            .Include(x => x.Members)
            .Include(x => x.League)
            .FirstOrDefault();

        if (disposeContext)
        {
            context.Dispose();
        }

        return season;
        
    }
}