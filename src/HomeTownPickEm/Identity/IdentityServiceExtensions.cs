using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using Microsoft.AspNetCore.Identity;

namespace Microsoft.AspNetCore.Builder.Extensions;

public static class IdentityServiceExtensions
{
    public static WebApplicationBuilder AddIdentity(this WebApplicationBuilder builder)
    {
        var identityBuilder = builder.Services.AddIdentityCore<ApplicationUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequiredLength = 4;
        });
        identityBuilder.AddEntityFrameworkStores<SqliteAppDbContext>();
        identityBuilder.AddSignInManager<SignInManager<ApplicationUser>>();
        identityBuilder.AddDefaultTokenProviders();

        builder.Services.AddIdentityCore<PostgresApplicationUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 4;
            }).AddEntityFrameworkStores<PostgreSqlIdentityAppDbContext>()
            .AddSignInManager<SignInManager<PostgresApplicationUser>>();
 
        
        return builder;
    }
}