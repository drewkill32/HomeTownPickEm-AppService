using HomeTownPickEm.Models;
using Microsoft.EntityFrameworkCore;

namespace SupabaseMigration.MigrationStrategies;

public class SeasonStrategy: IMigrationStrategy
{
    public string Key => "season";
    public async Task Migrate(DbMigrator migrator)
    {
        var prevBatchSize = migrator.BatchSize;
        migrator.BatchSize = 1;
        await migrator.Migrate<Season, int>(x => x.Id, x =>
        {
            return x.Include(y => y.Members)
                .Include(y => y.Teams)
                .AsTracking()
                .AsSplitQuery();
        }, async (seasons, context) =>
        {
            foreach (var season in seasons)
            {
                var postgresSeason = await context.Season
                    .AsTracking()
                    .Include(x => x.Teams)
                    .Include(x=> x.Members)
                    .AsSplitQuery()
                    .FirstOrDefaultAsync(x => x.Id == season.Id);

                var teamIds = season.Teams.Select(x => x.Id).ToArray();
                var teams = await context.Teams
                    .AsTracking()
                    .Where(x => teamIds.Contains(x.Id))
                    .ToArrayAsync();
                
                var userIds = season.Members.Select(x => x.Id)
                    .ToArray();
                
                var users = await context.Users
                    .AsTracking()
                    .Where(x => userIds.Contains(x.Id))
                    .ToArrayAsync();
                
                
                if (postgresSeason == null)
                {
                    var newSeason = new Season()
                    {
                        Active = season.Active,
                        Id = season.Id,
                        LeagueId = season.LeagueId,
                        Year = season.Year,
                    };
                    foreach (var team in teams)
                    {
                        newSeason.AddTeam(team);
                    }
                    foreach (var user in users)
                    {
                        newSeason.AddMember(user);
                    }
                    
                    context.Season.Add(newSeason);
                }
                else
                {
                    postgresSeason.Id = season.Id;
                    postgresSeason.Active = season.Active;
                    postgresSeason.LeagueId = season.LeagueId;
                    postgresSeason.Year = season.Year;
                    postgresSeason.Teams = teams;
                    postgresSeason.Members = users;
                }

                
                await context.SaveChangesAsync();

            }
            
        });
        migrator.BatchSize = prevBatchSize;
    }
}