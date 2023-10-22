using HomeTownPickEm.Models;

namespace SupabaseMigration.MigrationStrategies;

public class CalendarStrategy:IMigrationStrategy
{
    public string Key => "calendar";
    public async Task Migrate(DbMigrator migrator)
    {
        await migrator.Migrate<Calendar,CalendarKey>(x=> new CalendarKey(x.Season,x.Week,x.SeasonType));
    }
}