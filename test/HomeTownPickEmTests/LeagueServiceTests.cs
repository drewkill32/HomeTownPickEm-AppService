// using System;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
// using AutoFixture;
// using FluentAssertions;
// using HomeTownPickEm.Data;
// using HomeTownPickEm.Models;
// using HomeTownPickEm.Services;
// using Microsoft.EntityFrameworkCore;
// using Moq.AutoMock;
// using Xunit;
//
// namespace HomeTownPickEm
// {
//     public class LeagueServiceTests : IClassFixture<DatabaseFixture>
//     {
//         private readonly DatabaseFixture _databaseFixture;
//
//         public LeagueServiceTests(DatabaseFixture databaseFixture)
//         {
//             DbName = Guid.NewGuid().ToString();
//             _databaseFixture = databaseFixture;
//             _databaseFixture.SeedDatabase();
//
//             using var context = _databaseFixture.CreateDbContext(DbName);
//             var newLeague = Fixture.Freeze<Season>(cfg =>
//                 cfg.Without(x => x.Members)
//                     .Without(x => x.Picks)
//                     .Without(x => x.Teams));
//             context.Season.Add(newLeague);
//             context.SaveChanges();
//             LeagueId = newLeague.Id;
//         }
//
//         public Fixture Fixture { get; } = FixtureHelper.CreateDefaultFixture();
//
//         public AutoMocker Mocker { get; } = new();
//
//         public int LeagueId { get; set; }
//
//         public string DbName { get; set; }
//
//         [Fact]
//         public async Task AddMemberShould_AddMemberToLeagueWithPicks()
//         {
//             using var context = _databaseFixture.CreateDbContext(DbName);
//             var leagueService = CreateSutFixture(context);
//
//             var user = AddUser();
//
//             await leagueService.AddUserAsync(user.Id, CancellationToken.None);
//
//             var league = GetSeasonFromDb();
//
//             league.Members.Should().ContainSingle(x => x.Id == user.Id);
//
//             var games = GetGamesForTeam(user.TeamId.Value)
//                 .Select(x => x.Id).ToArray();
//
//             league.Picks.Select(x => x.GameId).Should()
//                 .BeEquivalentTo(games);
//         }
//
//         [Fact]
//         public async Task AddTeamShould_AddTeamToLeague()
//         {
//             var context = _databaseFixture.CreateDbContext(DbName);
//             var leagueService = CreateSutFixture(context);
//             var team = context.Teams.GetRandom();
//
//             await leagueService.AddTeamAsync(team.Id, CancellationToken.None);
//
//             var league = GetSeasonFromDb();
//
//             league.Teams.Should().ContainSingle(x => x.Id == team.Id);
//             league.Picks.Should().BeEmpty();
//         }
//
//         [Fact]
//         public async Task AddTwoUser_ShouldGetAllPicksForTwoTeam()
//         {
//             var msu = GetTeam(127);
//             var mich = GetTeam(130);
//             var michUser = AddUser(mich);
//             var msuUser = AddUser(msu);
//
//             var h2hGameId = 401282777;
//
//             //act
//             using (var context = _databaseFixture.CreateDbContext(DbName))
//             {
//                 var leagueService = CreateSutFixture(context);
//                 await leagueService.AddUserAsync(michUser.Id, CancellationToken.None);
//             }
//
//             using (var context = _databaseFixture.CreateDbContext(DbName))
//             {
//                 var leagueService = CreateSutFixture(context);
//                 await leagueService.AddUserAsync(msuUser.Id, CancellationToken.None);
//             }
//
//             var league = GetSeasonFromDb();
//             var msuGameIds = GetGamesForTeam(msu.Id)
//                 .Select(x => x.Id).ToArray();
//
//             var michGameIds = GetGamesForTeam(mich.Id)
//                 .Select(x => x.Id).ToArray();
//
//             //assert
//             foreach (var member in league.Members)
//             {
//                 var picks = league.Picks.Where(x => x.UserId == member.Id);
//
//                 var pickGamesId = picks.Select(x => x.GameId)
//                     .ToArray();
//
//                 //assert
//                 pickGamesId.Should()
//                     .Contain(msuGameIds);
//
//                 pickGamesId.Should()
//                     .Contain(michGameIds);
//
//                 pickGamesId
//                     .Where(x => x == h2hGameId)
//                     .Should().HaveCount(2);
//             }
//         }
//
//         [Fact]
//         public async Task AddUser_Should_AddTeam_IfExists()
//         {
//             var context = _databaseFixture.CreateDbContext(DbName);
//             var leagueService = CreateSutFixture(context);
//             var user = AddUser();
//             await leagueService.AddUserAsync(user.Id, CancellationToken.None);
//
//             var season = GetSeasonFromDb();
//
//             season.Members.Should().ContainSingle(x => x.Id == user.Id);
//             // all of the picks should 
//             season.Picks.Should().Match(x =>
//                 x.All(y => y.Game.HomeId == user.TeamId || y.Game.AwayId == user.TeamId));
//         }
//
//         [Fact]
//         public async Task CallingUpdate_MultipleTimes_IsIdempotent()
//         {
//             var mich = GetTeam(130);
//             var michUser = AddUser(mich);
//
//
//             //act
//             using (var context = _databaseFixture.CreateDbContext(DbName))
//             {
//                 var l = context.Season.Single();
//                 l.Teams.Add(mich);
//                 l.Members.Add(michUser);
//                 context.Update(l);
//                 context.SaveChanges();
//             }
//
//             using (var context = _databaseFixture.CreateDbContext(DbName))
//             {
//                 var leagueService = CreateSutFixture(context);
//                 await leagueService.Update(CancellationToken.None);
//             }
//
//             using (var context = _databaseFixture.CreateDbContext(DbName))
//             {
//                 var leagueService = CreateSutFixture(context);
//                 await leagueService.Update(CancellationToken.None);
//             }
//
//             var league = GetSeasonFromDb();
//
//             var michGameIds = GetGamesForTeam(mich.Id)
//                 .Select(x => x.Id).ToArray();
//
//
//             var pickGamesId = league.Picks.Select(x => x.GameId)
//                 .ToArray();
//
//
//             pickGamesId.Should()
//                 .BeEquivalentTo(michGameIds);
//         }
//
//
//         public ApplicationDbContext CreateContext()
//         {
//             return _databaseFixture.CreateDbContext(DbName);
//         }
//
//
//         [Fact]
//         public async Task GetPicks_ShouldGetAllGamesForSingleTeam()
//         {
//             var team = GetTeam();
//             using (var context = _databaseFixture.CreateDbContext(DbName))
//             {
//                 var leagueService = CreateSutFixture(context);
//                 await leagueService.AddTeamAsync(team.Id, CancellationToken.None);
//             }
//
//             Pick[] picks = null;
//             using (var context = _databaseFixture.CreateDbContext(DbName))
//             {
//                 var leagueService = CreateSutFixture(context);
//                 picks = (await leagueService.GetNewPicks(CancellationToken.None)).ToArray();
//             }
//
//             var games = GetGamesForTeam(team.Id)
//                 .Select(x => x.Id).ToArray();
//
//             picks.Select(x => x.GameId).Should()
//                 .BeEquivalentTo(games);
//         }
//
//         [Fact]
//         public async Task GetPicks_ShouldGetAllGamesForTwoTeam()
//         {
//             var msu = GetTeam(127);
//             var mich = GetTeam(130);
//             var h2hGameId = 401282777;
//
//             using (var context = _databaseFixture.CreateDbContext(DbName))
//             {
//                 var leagueService = CreateSutFixture(context);
//                 await leagueService.AddTeamAsync(msu.Id, CancellationToken.None);
//             }
//
//             using (var context = _databaseFixture.CreateDbContext(DbName))
//             {
//                 var leagueService = CreateSutFixture(context);
//                 await leagueService.AddTeamAsync(mich.Id, CancellationToken.None);
//             }
//
//             //act
//             Pick[] picks = null;
//             using (var context = _databaseFixture.CreateDbContext(DbName))
//             {
//                 var leagueService = CreateSutFixture(context);
//                 picks = (await leagueService.GetNewPicks(CancellationToken.None)).ToArray();
//             }
//
//             var msuGameIds = GetGamesForTeam(msu.Id)
//                 .Select(x => x.Id).ToArray();
//
//             var michGameIds = GetGamesForTeam(mich.Id)
//                 .Select(x => x.Id).ToArray();
//             var pickGamesId = picks.Select(x => x.GameId)
//                 .ToArray();
//
//
//             //assert
//             pickGamesId.Should()
//                 .Contain(msuGameIds);
//
//             pickGamesId.Should()
//                 .Contain(michGameIds);
//
//             pickGamesId
//                 .Where(x => x == h2hGameId)
//                 .Should().HaveCount(2);
//         }
//
//         [Fact]
//         public async Task UpdateShould_UpdateExistingPick()
//         {
//             var msu = GetTeam(127);
//             var mich = GetTeam(130);
//             var msuUser = AddUser(msu);
//             var michUser = AddUser(mich);
//
//             //add MSU user to league
//             using (var context = _databaseFixture.CreateDbContext(DbName))
//             {
//                 var leagueService = CreateSutFixture(context);
//                 await leagueService.AddUserAsync(msuUser.Id, CancellationToken.None);
//             }
//
//             //add Mich user to league
//             using (var context = _databaseFixture.CreateDbContext(DbName))
//             {
//                 var leagueService = CreateSutFixture(context);
//                 await leagueService.AddUserAsync(michUser.Id, CancellationToken.None);
//             }
//
//
//             var pick = GetSeasonFromDb().Picks.GroupBy(x => x.UserId).ToArray();
//
//
//             using (var context = _databaseFixture.CreateDbContext(DbName))
//             {
//             }
//
//
//             using (var context = _databaseFixture.CreateDbContext(DbName))
//             {
//                 var leagueService = CreateSutFixture(context);
//                 await leagueService.Update(CancellationToken.None);
//             }
//         }
//
//         private ApplicationUser AddUser()
//         {
//             Team team = null;
//             using (var context = _databaseFixture.CreateDbContext(DbName))
//             {
//                 team = context.Teams.First(x => x.Id == 130); // GO BLUE!
//             }
//
//             return AddUser(team);
//         }
//
//         private ApplicationUser AddUser(Team team)
//         {
//             using var context = _databaseFixture.CreateDbContext(DbName);
//             var user = Fixture.Build<ApplicationUser>()
//                 .With(x => x.TeamId, team.Id)
//                 .Without(x => x.Team)
//                 .Without(x => x.Seasons)
//                 .Create();
//             context.Users.Add(user);
//             context.SaveChanges();
//             return user;
//         }
//
//         private LeagueService CreateSutFixture(ApplicationDbContext context)
//         {
//             var leagueService = new LeagueService(LeagueId, context);
//             Mocker.Use(leagueService);
//             return leagueService;
//         }
//
//         private Game[] GetGamesForTeam(int teamId)
//         {
//             using var context = _databaseFixture.CreateDbContext(DbName);
//             return context.Games
//                 .Where(x => x.HomeId == teamId || x.AwayId == teamId).ToArray();
//         }
//
//
//         private Season GetSeasonFromDb()
//         {
//             using var context = _databaseFixture.CreateDbContext(DbName);
//             var season = context.Season
//                 .Include(x => x.Teams)
//                 .Include(x => x.Members)
//                 .Include(x => x.Picks)
//                 .ThenInclude(p => p.Game)
//                 .Single();
//             return season;
//         }
//
//         private Team GetTeam(int teamId)
//         {
//             using var context = CreateContext();
//             return context.Teams.Find(teamId);
//         }
//
//         private Team GetTeam()
//         {
//             using var context = CreateContext();
//             return context.Teams.GetRandom();
//         }
//     }
// }

