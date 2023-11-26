using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Data
{
    public class DatabaseMigrator
    {
        private readonly ApplicationDbContext _sqliteContext;
        private readonly PostgreSqlAppDbContext _postgreSqlAppDbContext;

        public DatabaseMigrator(SqliteAppDbContext sqliteContext, PostgreSqlAppDbContext postgreSqlAppDbContext)
        {
            _sqliteContext = sqliteContext;
            _postgreSqlAppDbContext = postgreSqlAppDbContext;
        }

        public async Task Init()
        {
            await ApplyMigrations(_sqliteContext);
            await ApplyMigrations(_postgreSqlAppDbContext);
        }

        private async Task ApplyMigrations(DbContext context)
        {
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }
        }
    }
}