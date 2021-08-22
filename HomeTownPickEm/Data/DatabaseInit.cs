using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HomeTownPickEm.Data
{
    public class DatabaseInit
    {
        private readonly IServiceProvider _provider;

        public DatabaseInit(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Task Init()
        {
            return ApplyMigrations<ApplicationDbContext>();
        }

        public Task Seed()
        {
            return Task.CompletedTask;
        }

        private async Task ApplyMigrations<TDbContext>() where TDbContext : DbContext
        {
            var context = _provider.GetService<TDbContext>();
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }
        }
    }
}