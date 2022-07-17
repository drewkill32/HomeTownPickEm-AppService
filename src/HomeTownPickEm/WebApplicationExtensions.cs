using HomeTownPickEm.Data;

namespace Microsoft.AspNetCore.Builder.Extensions;

public static class WebApplicationExtensions
{
    public static async Task RunStartup(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.GetService<IServiceScopeFactory>().CreateScope();
        var dbMigrator = ActivatorUtilities.CreateInstance<DatabaseMigrator>(scope.ServiceProvider);
        await dbMigrator.Init();
    }
}