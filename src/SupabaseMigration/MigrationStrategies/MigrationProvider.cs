namespace SupabaseMigration.MigrationStrategies;

public class MigrationProvider
{
    private readonly Dictionary<string, IMigrationStrategy> _strategies = new(StringComparer.InvariantCultureIgnoreCase);
    public MigrationProvider()
    {
        Add(new TeamStrategy());
        Add(new CalendarStrategy());
        Add(new GameStrategy());
        Add(new LeagueStrategy());
        Add(new UserStrategy());
        Add(new RoleStrategy());
        Add(new UserClaimStrategy());
        Add(new RoleClaimStrategy());
        Add(new UserLoginStrategy());
        Add(new UserRoleStrategy());
        Add(new UserTokenStrategy());
        Add(new SeasonStrategy());
        Add(new PickStrategy());
        Add(new PendingInvitesStrategy());
    }
    
    private void Add(IMigrationStrategy strategy)
    {
        _strategies.Add(strategy.Key, strategy);
    }
    
    
    public async Task Migrate(string[] keys, DbMigrator migrator)
    {
        if (keys.Length == 0 || keys.Contains("all", StringComparer.InvariantCultureIgnoreCase))
        {
            await MigrateAll(migrator);
            return;
        }
        
        foreach (var key in keys)
        {
            if (_strategies.TryGetValue(key, out var strategy))
            {
                await strategy.Migrate(migrator);
            }
        }
    }

    private async Task MigrateAll(DbMigrator migrator)
    {
        foreach (var strategy in _strategies.Values)
        {
            await strategy.Migrate(migrator);
        }
    }
}