using System;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeTownPickEm
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.GetService<IServiceProvider>().CreateScope())
            {
                var databaseInit = ActivatorUtilities.CreateInstance<DatabaseInit>(scope.ServiceProvider);
                await databaseInit.Init();
            }

            await host.RunAsync();
        }
    }
}