#region

using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Data;
using HomeTownPickEm.Services.DataSeed.User;
using Microsoft.Extensions.DependencyInjection.Extensions;

#endregion

namespace HomeTownPickEm.Services.DataSeed
{
    public static class SeederServiceExtensions
    {
        public static IServiceCollection AddSeeder<TSeeder>(this IServiceCollection services)
            where TSeeder : class, ISeeder
        {
            services.TryAddTransient<ISeeder, AggregateSeeder>();
            services.Add(ServiceDescriptor.Transient(_ => new SeederFactory(typeof(TSeeder))));
            return services;
        }

        private static IServiceCollection SeedDatabase(this IServiceCollection services,
            Func<ApplicationDbContext, CancellationToken, Task> execute)
        {
            services.AddTransient<ISeeder>(p => ActivatorUtilities.CreateInstance<FuncSeeder>(p, execute));
            return services;
        }

        public static IServiceCollection AddSeeders(this IServiceCollection services)
        {
            services.AddSeeder<LeagueSeeder>();
            services.AddSeeder<TeamSeeder>();
            services.AddSeeder<GameSeeder>();
            services.AddSeeder<UserSeeder>();
            services.AddSeeder<UserClaimSeeder>();
            services.AddSeeder<UserProfilePicSeeder>();
            services.AddSeeder<CalendarSeeder>();
            services.AddSeeder<SeasonSeeder>();
            services.AddSeeder<PickFixSeeder>();
            return services;
        }
    }
}