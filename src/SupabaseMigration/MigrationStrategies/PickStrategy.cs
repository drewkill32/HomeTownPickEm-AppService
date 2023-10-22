using HomeTownPickEm.Models;

namespace SupabaseMigration.MigrationStrategies;

public class PickStrategy: IMigrationStrategy
{
    public string Key => "pick";
    public async Task Migrate(DbMigrator migrator)
    {
        await migrator.Migrate<Pick,int>(x=> x.Id);
    }
}