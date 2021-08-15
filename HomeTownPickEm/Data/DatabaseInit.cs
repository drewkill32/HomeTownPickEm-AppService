using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.EntityFramework.Mappers;
using Client = IdentityServer4.Models.Client;
using Secret = IdentityServer4.Models.Secret;

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
            return Task.WhenAll(ApplyMigrations<PersistedGrantDbContext>(),
                ApplyMigrations<ConfigurationDbContext>(),
                ApplyMigrations<ApplicationDbContext>());
        }

        private async Task ApplyMigrations<TDbContext>() where TDbContext :DbContext
        {
            var context = _provider.GetService<TDbContext>();
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
            }
        }

        public async Task Seed()
        {
            var context = _provider.GetService<ConfigurationDbContext>();

            if (!context.Clients.Any())
            {
                var client = new Client()
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("SuperSecretPassword".Sha256()) },

                    AllowedScopes = { "weatherapi.read", "weatherapi.write" }
                };
                context.Clients.Add(client.ToEntity());
                await context.SaveChangesAsync();
            }
        }
    }
}