using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Application.Attributes;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using HomeTownPickEm.Services.Cfbd;
using MediatR;

namespace HomeTownPickEm.Application.Games.Commands
{
    public class LoadGames
    {
        [CacheRefresh]
        public class Command : GameRequest, IRequest
        {
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly ApplicationDbContext _context;
            private readonly IPublisher _publisher;
            private readonly ILogger<LoadGames> _logger;
            private readonly ICfbdHttpClient _httpClient;


            public Handler(ICfbdHttpClient httpClient, ApplicationDbContext context, IPublisher publisher, ILogger<LoadGames> logger)
            {
                _context = context;
                _publisher = publisher;
                _logger = logger;
                _httpClient = httpClient;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Loading Games for {Year}. Week: {Week}", request.Year, request.Week);
                var games = (await _httpClient.GetGames(request, cancellationToken))
                    .ToHashSet(new IdEqualityComparer<GameResponse>())
                    .Select(g => g.ToGame())
                    .ToArray();

                _logger.LogInformation("Loaded {Count} Games for {Year}. Week: {Week}", games.Length, request.Year, request.Week);
                _context.Games.AddOrUpdateRange(games);

                await _context.SaveChangesAsync(cancellationToken);
                await _publisher.Publish(new GamesUpdatedNotification
                {
                    Week = request.Week,
                    Year = request.Year
                }, cancellationToken);
                return Unit.Value;
                
            }
            
        }
    }
}