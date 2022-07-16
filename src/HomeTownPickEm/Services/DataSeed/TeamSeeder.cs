using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Application.Teams.Commands.LoadTeams;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeTownPickEm.Services.DataSeed
{
    public class TeamSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public TeamSeeder(ApplicationDbContext context,
            IMediator mediator,
            ILogger<TeamSeeder> logger)
        {
            _context = context;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Seed(CancellationToken cancellationToken)
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