using System;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Abstract.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HomeTownPickEm.Services.DataSeed
{
    public class SeederWorkerService : BackgroundService
    {
        private readonly ILogger<SeederWorkerService> _logger;
        private readonly ISeeder _seeder;


        public SeederWorkerService(ISeeder seeder, ILogger<SeederWorkerService> logger)
        {
            _seeder = seeder;
            _logger = logger;
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                try
                {
                    await _seeder.Seed(stoppingToken);
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