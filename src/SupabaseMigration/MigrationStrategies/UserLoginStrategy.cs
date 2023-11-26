using Microsoft.AspNetCore.Identity;

namespace SupabaseMigration.MigrationStrategies;

public class UserLoginStrategy: IMigrationStrategy
{
    public string Key => "login";
    public async Task Migrate(DbMigrator migrator)
    {
        await migrator.Migrate<IdentityUserLogin<string>,UserLoginKey>(x=> new UserLoginKey(x.LoginProvider,x.ProviderKey));
    }
    private record UserLoginKey(string LoginProvider, string ProviderKey);
}