using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using Microsoft.AspNetCore.Identity;

namespace Microsoft.AspNetCore.Builder.Extensions;

public static class IdentityServiceExtensions
{
    public static WebApplicationBuilder AddIdentity(this WebApplicationBuilder builder)
    {
        
        builder.Services.AddIdentityCore<ApplicationUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 4;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddDefaultTokenProviders();
 
        
        return builder;
    }
}