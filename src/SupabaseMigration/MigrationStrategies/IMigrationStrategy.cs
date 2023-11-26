namespace SupabaseMigration.MigrationStrategies;

public interface IMigrationStrategy
{
    string Key { get; }
    
    Task Migrate(DbMigrator migrator);
}