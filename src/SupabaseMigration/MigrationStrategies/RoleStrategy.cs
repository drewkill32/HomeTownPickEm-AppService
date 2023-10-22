using Microsoft.AspNetCore.Identity;

namespace SupabaseMigration.MigrationStrategies;

public class RoleStrategy: IMigrationStrategy
{
    public string Key => "role";
    public async Task Migrate(DbMigrator migrator)
    {
        await migrator.Migrate<IdentityRole,string>(x=> x.Id);
    }
}