using HomeTownPickEm.Models;

namespace SupabaseMigration.MigrationStrategies;

public class UserStrategy: IMigrationStrategy
{
    public string Key => "user";
    public async Task Migrate(DbMigrator migrator)
    {
        await migrator.Migrate<ApplicationUser,string>(x=> x.Id);
    }
}
