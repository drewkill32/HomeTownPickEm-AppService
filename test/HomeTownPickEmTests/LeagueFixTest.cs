using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using HomeTownPickEm.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace HomeTownPickEm;

public class LeagueFixTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly ApplicationDbContext _context;

    public LeagueFixTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
            .UseSqlite(@"DataSource=C:\Users\akillion\Code\Hometown\src\HomeTownPickEm\app.db;Cache=Shared")
            .Options;
        _context = new ApplicationDbContext(options);
    }

    [Fact]
    public async Task FixPicks()
    {
        // first get all the games that are head to head

        var teams = await _context.Season.Where(x => x.Year == "2022")
            .Include(x => x.Members)
            .SelectMany(x => x.Teams)
            .ToArrayAsync();

        //get all of the games including picks that include those teams
        var games = await _context.Games.Where(x => x.Season == "2022")
            .WhereTeamsArePlaying(teams)
            .Include(x => x.Away)
            .Include(x => x.Home)
            .Include(x => x.Picks)
            .ThenInclude(p => p.User)
            .ToArrayAsync();

        foreach (var game in games)
        {
            var isHeadToHead = IsHeadToHead(game, teams);
            var userPicks = game.Picks.GroupBy(x => new
            {
                x.UserId, x.User.Name
            }).ToArray();
            userPicks.Should().HaveCount(18);

            foreach (var userPick in userPicks)
            {
                var picks = userPick
                    .OrderBy(p => p.Id)
                    .ToArray();

                picks.Should().HaveCount(isHeadToHead ? 2 : 1);
            }
        }
    }

    private bool IsHeadToHead(Game game, Team[] teams)
    {
        var teamIds = teams.Select(x => x.Id).ToArray();
        return teamIds.Contains(game.AwayId) && teamIds.Contains(game.HomeId);
    }
}