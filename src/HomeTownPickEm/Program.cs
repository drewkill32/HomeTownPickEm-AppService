using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Common.Behaviors;
using HomeTownPickEm.Config;
using HomeTownPickEm.Data;
using HomeTownPickEm.Filters;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using HomeTownPickEm.Services;
using HomeTownPickEm.Services.Cfbd;
using HomeTownPickEm.Services.CFBD;
using HomeTownPickEm.Services.DataSeed;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

AddDbContext(builder);


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilterAttribute>();
    var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddRazorPages();

AddIdentity(builder);

AddJwt(builder);

AddSwagger(builder);

AddServices(builder);

builder.Services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

}
else
{
    app.UseHttpsRedirection();
}


app.UseStaticFiles();
app.UseSpaStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();


app.UseSwagger();
app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    opt.RoutePrefix = "api";
});


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});


app.UseSpa(spa =>
{
    spa.Options.SourcePath = "ClientApp";

    if (app.Environment.IsDevelopment())
    {
        spa.UseReactDevelopmentServer(npmScript: "start");
    }
});

await RunStartup(app);

await app.RunAsync();

void AddDbContext(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection")));

    webApplicationBuilder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

void AddIdentity(WebApplicationBuilder builder1)
{
    var identityBuilder = builder1.Services.AddIdentityCore<ApplicationUser>(opt =>
    {
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequiredLength = 4;
    });
    identityBuilder.AddEntityFrameworkStores<ApplicationDbContext>();
    identityBuilder.AddSignInManager<SignInManager<ApplicationUser>>();

    identityBuilder.AddDefaultTokenProviders();
}

void AddJwt(WebApplicationBuilder webApplicationBuilder1)
{
    webApplicationBuilder1.Services.AddScoped<IJwtGenerator, JwtGenerator>();
    webApplicationBuilder1.Services.AddScoped<IUserAccessor, UserAccessor>();
    webApplicationBuilder1.Services.AddHostedService<SeederWorkerService>();
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(webApplicationBuilder1.Configuration["TokenKey"]));

    webApplicationBuilder1.Services.AddAuthorization(opt =>
    {
        opt.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();
    });
    webApplicationBuilder1.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new ()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key
        };
    });
   
}

void AddSwagger(WebApplicationBuilder builder2)
{
    builder.Services.AddEndpointsApiExplorer();
    builder2.Services.AddSwaggerGen(opt =>
    {
        opt.CustomSchemaIds(t =>
            t.IsNested ? $"{t.DeclaringType.Name}_{t.Name}" : t.Name);
        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });

        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });
}

void AddServices(WebApplicationBuilder webApplicationBuilder2)
{
    webApplicationBuilder2.Services.AddSeeders();
    webApplicationBuilder2.Services.AddScoped<ILeagueServiceFactory, LeagueServiceFactory>();
    webApplicationBuilder2.Services.Configure<SendGridSettings>(
        webApplicationBuilder2.Configuration.GetSection(SendGridSettings.SettingsKey));
    webApplicationBuilder2.Services.AddSingleton<IEmailSender, SendGridEmailSender>();

    webApplicationBuilder2.Services.Configure<CfbdSettings>(
        webApplicationBuilder2.Configuration.GetSection(CfbdSettings.SettingsKey));
    webApplicationBuilder2.Services.AddHttpClient(CfbdSettings.SettingsKey, (provider, client) =>
    {
        var settings = provider.GetRequiredService<IOptions<CfbdSettings>>().Value;
        client.BaseAddress = new Uri(settings.BaseUrl);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.Key);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    });

    webApplicationBuilder2.Services.AddHttpClient<ICfbdHttpClient, CfbdHttpClient>((provider, client) =>
    {
        var settings = provider.GetRequiredService<IOptions<CfbdSettings>>().Value;
        client.BaseAddress = new Uri(settings.BaseUrl);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.Key);
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    });

    webApplicationBuilder2.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
    webApplicationBuilder2.Services.AddMediatR(Assembly.GetExecutingAssembly());
    webApplicationBuilder2.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
    webApplicationBuilder2.Services.AddScoped<GameTeamRepository>();
}

async Task RunStartup(WebApplication webApplication)
{
    using (var scope = webApplication.Services.GetService<IServiceProvider>().CreateScope())
    {
        var seeder = ActivatorUtilities.CreateInstance<DatabaseInit>(scope.ServiceProvider);
        await seeder.Init();
    }
}
