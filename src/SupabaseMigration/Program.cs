// See https://aka.ms/new-console-template for more information

using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SupabaseMigration;
using SupabaseMigration.MigrationStrategies;


var app = Startup.Build(args, builder =>
{
    
    builder.Services.AddDbContext<PostgreSqlAppDbContext>(options =>
    {
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
            .UseNpgsql(builder.Configuration.GetConnectionString("Supabase"));
    });
    builder.Configuration.AddJsonFile("appsettings.json", false, true);
    
} );

try
{
    using var scope = app.Services.CreateScope();
    
    var postgreSqlDbContext = scope.ServiceProvider.GetRequiredService<PostgreSqlAppDbContext>();
    var sqlLiteDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await postgreSqlDbContext.Database.MigrateAsync();
    await sqlLiteDbContext.Database.MigrateAsync();
    
    var migrator = ActivatorUtilities.CreateInstance<DbMigrator>(scope.ServiceProvider);

    var provider = new MigrationProvider();

    await provider.Migrate(args, migrator);
    

}
catch (Exception e)
{
    var prevColor = Console.ForegroundColor;
    var prevBgColor = Console.BackgroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.BackgroundColor = ConsoleColor.Black;
    Console.WriteLine(e);
    Console.ForegroundColor = prevColor;
    Console.BackgroundColor = prevBgColor;
}


Console.WriteLine("Press any key to continue...");  
Console.ReadKey();



