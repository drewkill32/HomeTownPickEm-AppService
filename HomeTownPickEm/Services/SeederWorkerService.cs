using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HomeTownPickEm.Services
{
    public class SeederWorkerService : BackgroundService
    {
        private readonly ILogger<SeederWorkerService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;


        public SeederWorkerService(
            IServiceScopeFactory scopeFactory, ILogger<SeederWorkerService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                using var scope = _scopeFactory.CreateScope();
                var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
                try
                {
                    await seeder.SeedAsync(stoppingToken);
                    _logger.LogInformation("Successfully seeded database");
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Unable to seed database. {ErrorMessage}", ex.Message);
                }
            }, stoppingToken);
        }
    }
}