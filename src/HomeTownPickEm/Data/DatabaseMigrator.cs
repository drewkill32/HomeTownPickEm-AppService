using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Data
{
    public class DatabaseMigrator
    {
        private readonly ApplicationDbContext _context;

        public DatabaseMigrator(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Init()
        {
            if ((await _context.Database.GetPendingMigrationsAsync()).Any())
            {
                await _context.Database.MigrateAsync();
            }
        }
    }
}