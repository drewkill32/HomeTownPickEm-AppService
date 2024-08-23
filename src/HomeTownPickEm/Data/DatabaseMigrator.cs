using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Data;

public class DatabaseMigrator
{
    private readonly ApplicationDbContext _dbContext;

    public DatabaseMigrator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Init()
    {
        await ApplyMigrations(_dbContext);
    }

    private async Task ApplyMigrations(DbContext context)
    {
        if ((await context.Database.GetPendingMigrationsAsync()).Any()) await context.Database.MigrateAsync();
    }
}