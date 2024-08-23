using HomeTownPickEm.Data;
using HomeTownPickEm.Filters;
using HomeTownPickEm.Hubs;
using HomeTownPickEm.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Microsoft.AspNetCore.Builder.Extensions;

public class Startup
{
    public static WebApplication Build(string[] args, Action<WebApplicationBuilder> configure = null)
    {
        
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.AddAzureWebAppDiagnostics();
        
        builder.AddDbContext();


        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilterAttribute>();
            var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        });

        builder.Services.AddRazorPages();

        var spaUrl = builder.Configuration
            .GetValue("Spa:Url", "http://localhost:3000;https://localhost:3000;https://localhost:5001").Split(";");
        builder.Services.Configure<OriginOptions>(options => { options.AllowedOrigins = spaUrl; });
        builder.Services.AddCors(ctx =>
        {
            ctx.AddDefaultPolicy(ply =>
                ply.WithOrigins(spaUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
            );
        });

        builder.AddIdentity();

        builder.AddJwt();

        builder.AddSwagger();

        builder.Services.AddApplicationInsightsTelemetry();

        builder.AddServices();

        configure?.Invoke(builder);
        return builder.Build();
    }

    public static async Task Run(WebApplication app)
    {
        
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseHttpsRedirection();
        }

        app.UseCors();


        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();
        app.UseAuthentication();


        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            opt.RoutePrefix = "";
        });


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
            endpoints.MapHub<CacheHub>("/hubs/cache");
        });




        await app.RunStartup();

        await app.RunAsync();
    }
}