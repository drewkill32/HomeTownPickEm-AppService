using Microsoft.AspNetCore.Identity;

namespace SupabaseMigration.MigrationStrategies;

public class UserRoleStrategy: IMigrationStrategy
{
    public string Key => "user-role";
    public async Task Migrate(DbMigrator migrator)
    {
        await migrator.Migrate<IdentityUserRole<string>,UserRoleKey>(x=> new UserRoleKey(x.UserId, x.RoleId));
    }
    private record UserRoleKey(string UserId, string RoleId);
}