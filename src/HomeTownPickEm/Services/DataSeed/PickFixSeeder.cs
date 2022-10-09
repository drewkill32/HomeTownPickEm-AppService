using System.Diagnostics;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using HomeTownPickEm.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Services.DataSeed;

public class PickFixSeeder : ISeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PickFixSeeder> _logger;

    public PickFixSeeder(ApplicationDbContext context, ILogger<PickFixSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Seed(CancellationToken cancellationToken)
    {
        // first get all the games that are head to head
        var teams = await _context.Season.Where(x => x.Year == "2022")
            .AsTracking()
            .Include(x => x.Members)
            .SelectMany(x => x.Teams)
            .ToArrayAsync(cancellationToken);


        //get all of the games including picks that include those teams
        var games = await _context.Games.Where(x => x.Season == "2022")
            .AsTracking()
            .WhereTeamsArePlaying(teams)
            .Include(x => x.Away)
            .Include(x => x.Home)
            .Include(x => x.Picks)
            .ThenInclude(p => p.User)
            .ToArrayAsync(cancellationToken);

        var pointPicks = new List<Pick>();
        foreach (var game in games)
        {
            var isHeadToHead = IsHeadToHead(game, teams);
            var userPicks = game.Picks.GroupBy(x => new
            {
                x.UserId, x.User.Name
            }).ToArray();
            Debug.Assert(userPicks.Length == 18);

            foreach (var userPick in userPicks)
            {
                var picks = userPick
                    .OrderBy(p => p.Id)
                    .ToArray();


                var selectedTeamId = picks[0].SelectedTeamId;
                if (isHeadToHead && picks.Length > 2)
                {
                    //we should never have three games
                    //verify that they are all for the same team;

                    if (picks.Any(x => x.SelectedTeamId != selectedTeamId))
                    {
                        //the user split the picks find the pick with the higher id and delete it
                        var p = picks
                            .GroupBy(pick => pick.SelectedTeamId)
                            .Where(selectedPicks => selectedPicks.Count() > 1)
                            .SelectMany(x => x);

                        var pickToDelete = p.Last();
                        _context.Pick.Remove(pickToDelete);
                        if (pickToDelete.Points > 0)
                        {
                            pointPicks.Add(pickToDelete);
                            _logger.LogWarning("Removing {PickId} for {User} on Game {Game} and {Point} points",
                                pickToDelete.Id, pickToDelete.User.Name, pickToDelete.Game, pickToDelete.Points);
                        }
                    }
                    else
                    {
                        var p = picks.Last();
                        _context.Pick.Remove(p);
                        if (p.Points > 0)
                        {
                            pointPicks.Add(p);
                            _logger.LogWarning("Removing {PickId} for {User} on Game {Game} and {Point} points",
                                p.Id, p.User.Name, p.Game, p.Points);
                        }
                    }
                }
                else if (!isHeadToHead && picks.Length != 1)
                {
                    Debug.Assert(picks.All(pick => pick.SelectedTeamId == selectedTeamId));
                    var pickToKeep = picks.First();
                    var picksToDelete = picks.Where(x => x.Id != pickToKeep.Id).ToArray();
                    _context.Pick.RemoveRange(picksToDelete);
                    foreach (var pick in picksToDelete)
                    {
                        if (pick.Points > 0)
                        {
                            pointPicks.Add(pick);
                            _logger.LogWarning("Removing {PickId} for {User} on Game {Game} and {Point} points",
                                pick.Id, pick.User.Name, pick.Game, pick.Points);
                        }
                    }
                }
            }
        }


        await _context.SaveChangesAsync(cancellationToken);
    }

    private bool IsHeadToHead(Game game, Team[] teams)
    {
        var teamIds = teams.Select(x => x.Id).ToArray();
        return teamIds.Contains(game.AwayId) && teamIds.Contains(game.HomeId);
    }
}