using Microsoft.AspNetCore.Identity;

namespace SupabaseMigration.MigrationStrategies;

public class UserTokenStrategy: IMigrationStrategy
{
    public string Key => "user-token";
    public async Task Migrate(DbMigrator migrator)
    {
        await migrator.Migrate<IdentityUserToken<string>,UserTokenKey>(x=> new UserTokenKey(x.UserId, x.LoginProvider, x.Name));
    }
    
    private record UserTokenKey(string UserId, string LoginProvider, string Name);
}