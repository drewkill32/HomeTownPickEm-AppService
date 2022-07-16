using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using HomeTownPickEm.Services;
using MediatR;

namespace HomeTownPickEm.Application.Leagues.Commands
{
    public class UpdateLeague
    {
        public class Command : IRequest
        {
            public int LeagueId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly ApplicationDbContext _context;
            private readonly ILeagueServiceFactory _leagueServiceFactory;

            public CommandHandler(ApplicationDbContext context, ILeagueServiceFactory leagueServiceFactory)
            {
                _context = context;
                _leagueServiceFactory = leagueServiceFactory;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var leagueService = _leagueServiceFactory.Create(request.LeagueId);

                await leagueService.Update(cancellationToken);
                return Unit.Value;
            }
        }
    }
}