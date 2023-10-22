using HomeTownPickEm.Models;

namespace SupabaseMigration.MigrationStrategies;

public class TeamStrategy: IMigrationStrategy
{
    public string Key => "team";
    public async Task Migrate(DbMigrator migrator)
    {
        await migrator.Migrate<Team,int>(x => x.Id);
    }
}