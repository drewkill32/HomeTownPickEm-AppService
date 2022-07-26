using System;
using System.Linq;
using AutoFixture;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm;

public class DatabaseFixture
{
    private readonly string _name;
    private readonly Fixture _fixture = FixtureHelper.CreateDefaultFixture();

    public DatabaseFixture()
    {
        _name = Guid.NewGuid().ToString();
        SeedDatabase();
    }

    public ApplicationDbContext CreateDbContext(Action<DbContextOptionsBuilder> optionsAction = null)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(_name);
        optionsAction?.Invoke(options);
        return new ApplicationDbContext(options.Options);
    }

    public ApplicationDbContext CreateTrackingDbContext(Action<DbContextOptionsBuilder> optionsAction = null)
    {
        return CreateDbContext(opt => opt.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll));
    }

    private void SeedDatabase()
    {
        using var context = CreateDbContext();
        {
            DatabaseSeeder.SeedTeams(context);
            DatabaseSeeder.SeedGames(context);
        }
    }

    public Game[] GetGamesForTeam(int teamId)
    {
        using var context = CreateDbContext();
        return context.Games
            .Where(x => x.HomeId == teamId || x.AwayId == teamId).ToArray();
    }

    public Team GetTeam(int id)
    {
        using var context = CreateDbContext();
        return context.Teams.Find(id);
    }

    public ApplicationUser CreateUser()
    {
        using var context = CreateDbContext();
        var user = _fixture.Build<ApplicationUser>()
            .Without(x => x.Seasons)
            .Without(x => x.TeamId)
            .Without(x => x.Team)
            .Create();

        context.Users.Add(user);
        context.SaveChanges();
        return user;
    }


    public Season CreateSeason()
    {
        using var context = CreateDbContext();
        var season = _fixture.Build<Season>()
            .Without(x => x.Members)
            .Without(x => x.Picks)
            .Without(x => x.Teams)
            .Create();

        season.LeagueId = season.League.Id;

        context.Season.Add(season);
        context.SaveChanges();
        return season;
    }


    public League CreateLeague()
    {
        using var context = CreateDbContext();
        var league = _fixture.Build<League>()
            .Without(x => x.Seasons)
            .Create();
        context.League.Add(league);
        context.SaveChanges();
        return league;
    }
}