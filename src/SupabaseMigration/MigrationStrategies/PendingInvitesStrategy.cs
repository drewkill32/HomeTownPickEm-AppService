using HomeTownPickEm.Models;

namespace SupabaseMigration.MigrationStrategies;

public class PendingInvitesStrategy: IMigrationStrategy
{
    public string Key => "pending-invites";
    public async Task Migrate(DbMigrator migrator)
    {
        await migrator.Migrate<PendingInvite,PendingInviteKey>(x=> new PendingInviteKey(x.LeagueId,x.Season, x.UserId));
    }

    private record PendingInviteKey(int LeagueId, string Season, string UserId);
}