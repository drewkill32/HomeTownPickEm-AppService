using HomeTownPickEm.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.AddDbContext();


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilterAttribute>();
    var policy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddRazorPages();

var spaUrl = builder.Configuration.GetValue("Spa:Url", "http://localhost:3000");
builder.Services.AddCors(ctx =>
{
    ctx.AddDefaultPolicy(ply =>
        ply.WithOrigins(spaUrl).AllowAnyHeader().AllowAnyMethod());
});

builder.AddIdentity();

builder.AddJwt();

builder.AddSwagger();

builder.AddServices();

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

app.UseCors();


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
        spa.UseProxyToSpaDevelopmentServer(spaUrl);
    }
});

await app.RunStartup();

await app.RunAsync();

