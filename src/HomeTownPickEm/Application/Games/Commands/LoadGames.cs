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
            private readonly ICfbdHttpClient _httpClient;


            public Handler(ICfbdHttpClient httpClient, ApplicationDbContext context, IPublisher publisher)
            {
                _context = context;
                _publisher = publisher;
                _httpClient = httpClient;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var games = (await _httpClient.GetGames(request, cancellationToken))
                    .ToHashSet(new IdEqualityComparer<GameResponse>())
                    .Select(g => g.ToGame())
                    .ToArray();


                _context.Games.AddOrUpdateRange(games, q =>
                    q.Where(g => g.Season == request.Year && g.SeasonType == request.SeasonType)
                        .WhereWeekIs(request.Week));

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