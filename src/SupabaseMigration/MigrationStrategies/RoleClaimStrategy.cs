using Microsoft.AspNetCore.Identity;

namespace SupabaseMigration.MigrationStrategies;

public class RoleClaimStrategy: IMigrationStrategy
{
    public string Key => "role-claim";
    public async Task Migrate(DbMigrator migrator)
    {
        await migrator.Migrate<IdentityRoleClaim<string>,int>(x=> x.Id);
    }
}