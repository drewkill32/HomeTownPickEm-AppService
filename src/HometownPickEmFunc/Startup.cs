using System;
using System.Net.Http.Headers;
using HometownPickEmFunc;
using HometownPickEmFunc.CFBD;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(Startup))]

namespace HometownPickEmFunc
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddLogging();
            builder.Services.AddOptions<CFBDSettings>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection(CFBDSettings.SettingsKey).Bind(settings);
                });

            builder.Services.AddScoped<IUpdateGamesService, UpdateGamesService>();
            builder.Services.AddHttpClient(CFBDSettings.SettingsKey, (provider, client) =>
            {
                var settings = provider.GetService<IOptions<CFBDSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.Key);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}