using HomeTownPickEm.Models;

namespace SupabaseMigration.MigrationStrategies;

public class GameStrategy: IMigrationStrategy
{
    public string Key => "game";
    public async Task Migrate(DbMigrator migrator)
    {
        await migrator.Migrate<Game,int>(x => x.Id);
    }
    
}