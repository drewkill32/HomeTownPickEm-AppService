using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Abstract.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HomeTownPickEm.Services.DataSeed
{
    public class AggregateSeeder : ISeeder
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IEnumerable<SeederFactory> _seederFactories;


        public AggregateSeeder(IServiceScopeFactory scopeFactory,
            IEnumerable<SeederFactory> seederFactories)
        {
            _scopeFactory = scopeFactory;
            _seederFactories = seederFactories;
        }


        public async Task Seed(CancellationToken cancellationToken)
        {
            foreach (var factory in _seederFactories)
            {
                using var scope = _scopeFactory.CreateScope();
                var seeder = factory.Create(scope.ServiceProvider);
                await seeder.Seed(cancellationToken);
            }
        }
    }
}