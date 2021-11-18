using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using HomeTownPickEm.Services;
using MediatR;

namespace HomeTownPickEm.Application.Leagues.Commands
{
    public class UpdateLeagueCommandHandler : IRequestHandler<UpdateLeagueCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILeagueServiceFactory _leagueServiceFactory;

        public UpdateLeagueCommandHandler(ApplicationDbContext context, ILeagueServiceFactory leagueServiceFactory)
        {
            _context = context;
            _leagueServiceFactory = leagueServiceFactory;
        }

        public async Task<Unit> Handle(UpdateLeagueCommand request, CancellationToken cancellationToken)
        {
            var leagueService = _leagueServiceFactory.Create(request.LeagueId);

            await leagueService.Update(cancellationToken);
            return Unit.Value;
        }
    }

    public class UpdateLeagueCommand : IRequest
    {
        public int LeagueId { get; set; }
    }
}