using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeTownPickEm.Services
{
    public class SeederWorkerService : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;


        public SeederWorkerService(
            IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<Seeder>();
            await Task.Run(async () => await seeder.SeedAsync(cancellationToken), cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}