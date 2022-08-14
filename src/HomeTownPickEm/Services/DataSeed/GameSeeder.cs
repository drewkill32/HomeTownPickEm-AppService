using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Application.Games.Commands;
using HomeTownPickEm.Application.Teams.Commands.LoadTeams;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Services.DataSeed
{
    public class GameSeeder : ISeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GameSeeder> _logger;
        private readonly IMediator _mediator;

        public GameSeeder(ApplicationDbContext context, IMediator mediator, ILogger<GameSeeder> logger)
        {
            _context = context;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Seed(CancellationToken cancellationToken)
        {
            var year = DateTime.Now.Year.ToString();
            if (!_context.Games.Any(g => g.Season == year))
            {
                await _mediator.Send(new LoadTeams.Command(), cancellationToken);
                await _mediator.Send(new LoadGames.Command(), cancellationToken);
                var games = await _context.Games.CountAsync(cancellationToken);
                _logger.LogInformation("Added {Count} games", games);
            }
        }
    }
}