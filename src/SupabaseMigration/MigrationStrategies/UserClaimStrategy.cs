using Microsoft.AspNetCore.Identity;

namespace SupabaseMigration.MigrationStrategies;

public class UserClaimStrategy: IMigrationStrategy
{
    public string Key => "user-claim";
    public async Task Migrate(DbMigrator migrator)
    {
        await migrator.Migrate<IdentityUserClaim<string>,int>(x=> x.Id);
    }
}