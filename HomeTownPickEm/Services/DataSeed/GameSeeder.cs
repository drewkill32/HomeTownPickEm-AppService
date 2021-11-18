using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Application.Games.Commands;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            if (!_context.Games.Any())
            {
                await _mediator.Send(new LoadGamesCommand(), cancellationToken);
                var games = await _context.Games.CountAsync(cancellationToken);
                _logger.LogInformation("Added {Count} games", games);
            }
        }
    }
}