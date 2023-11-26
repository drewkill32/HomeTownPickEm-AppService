using HomeTownPickEm.Models;

namespace SupabaseMigration.MigrationStrategies;

public class LeagueStrategy: IMigrationStrategy
{
    public string Key => "league";
    public async Task Migrate(DbMigrator migrator)
    {
        await migrator.Migrate<League,int>(x=> x.Id);
    }
}