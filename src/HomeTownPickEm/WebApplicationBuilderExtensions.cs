﻿using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Common.Behaviors;
using HomeTownPickEm.Config;
using HomeTownPickEm.Data;
using HomeTownPickEm.Security;
using HomeTownPickEm.Services;
using HomeTownPickEm.Services.Cfbd;
using HomeTownPickEm.Services.CFBD;
using HomeTownPickEm.Services.DataSeed;
using HomeTownPickEm.Services.Dev;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;

namespace Microsoft.AspNetCore.Builder.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        return builder;
    }



    public static WebApplicationBuilder AddJwt(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IUserAccessor, UserAccessor>();
        builder.Services.AddHostedService<SeederWorkerService>();

        builder.Services.AddAuthorization(opt =>
        {
            opt.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        });
        var jwtOptions = new JwtOptions();
        builder.Configuration.GetSection("jwt").Bind(jwtOptions);
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("jwt"));
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {
            opt.SaveToken = true;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                ClockSkew = builder.Environment.IsDevelopment() ? TimeSpan.Zero : TimeSpan.FromMinutes(5),
                ValidateIssuerSigningKey = true,


                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key))
            };
        });

        return builder;
    }

    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opt =>
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

        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSeeders();
        builder.Services.AddScoped<ILeagueServiceFactory, LeagueServiceFactory>();
        builder.Services.Configure<SendGridSettings>(
            builder.Configuration.GetSection(SendGridSettings.SettingsKey));


        builder.Services.Configure<CfbdSettings>(
            builder.Configuration.GetSection(CfbdSettings.SettingsKey));
        builder.Services.AddHttpClient(CfbdSettings.SettingsKey, (provider, client) =>
        {
            var settings = provider.GetRequiredService<IOptions<CfbdSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.Key);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        }).AddPolicyHandler(GetRetryPolicy());

        builder.Services.AddHttpClient<ICfbdHttpClient, CfbdHttpClient>((provider, client) =>
        {
            var settings = provider.GetRequiredService<IOptions<CfbdSettings>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.Key);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.Timeout = TimeSpan.FromSeconds(5);
        }).AddPolicyHandler(GetRetryPolicy());

        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
        builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
        builder.Services.AddScoped<GameTeamRepository>();
        builder.Services.AddSingleton(BackgroundWorkerQueue.Instance);
        builder.Services.AddHostedService<BackgroundWorker>();

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddSingleton<ISystemDate, DevSystemDate>();
            builder.Services.AddSingleton<IEmailSender, DevEmailSender>();
        }
        else
        {
            builder.Services.AddTransient<ISystemDate, SystemDate>();
            builder.Services.AddSingleton<IEmailSender, SendGridEmailSender>();
        }

        return builder;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                retryAttempt)));
    }
}