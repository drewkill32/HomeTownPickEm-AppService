using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Calendar.Commands;
using HomeTownPickEm.Application.Games.Commands;
using HomeTownPickEm.Application.Teams.Commands.LoadTeams;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HomeTownPickEm.Services
{
    public class Seeder
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public Seeder(IWebHostEnvironment env,
            IMediator mediator, ApplicationDbContext context, ILogger<DatabaseInit> logger)
        {
            _env = env;
            _mediator = mediator;
            _context = context;
            _logger = logger;
        }


        public async Task SeedAsync(CancellationToken cancellationToken)
        {
            await AddLeagues(cancellationToken);
            await AddTeams(cancellationToken);
            await AddCalendar(cancellationToken);
            await AddGames(cancellationToken);
        }

        private async Task AddCalendar(CancellationToken cancellationToken)
        {
            if (!_context.Calendar.Any())
            {
                await _mediator.Send(new LoadCalendar.Command(), cancellationToken);
                var calendars = await _context.Calendar.CountAsync(cancellationToken);
                _logger.LogInformation("Added {Count} Calendar weeks", calendars);
            }
        }

        private async Task AddGames(CancellationToken cancellationToken)
        {
            if (!_context.Games.Any())
            {
                await _mediator.Send(new LoadGames.Command(), cancellationToken);
                var games = await _context.Games.CountAsync(cancellationToken);
                _logger.LogInformation("Added {Count} games", games);
            }
        }

        private async ValueTask AddLeagues(CancellationToken cancellationToken)
        {
            if (!_env.IsDevelopment())
            {
                return;
            }

            if (!_context.League.Any())
            {
                _context.League.Add(new League
                {
                    Name = "Test League",
                    Season = DateTime.Now.Year.ToString()
                });
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Added Test League");
            }
        }

        private async Task AddTeams(CancellationToken cancellationToken)
        {
            if (!_context.Teams.Any())
            {
                try
                {
                    await _mediator.Send(new LoadTeams.Command(), cancellationToken);
                    var teams = await _context.Teams.CountAsync(cancellationToken);
                    _logger.LogInformation("Added {Count} Teams", teams);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error Adding Teams. {ErrorMessage}", e.Message);
                }
            }
        }
    }
}