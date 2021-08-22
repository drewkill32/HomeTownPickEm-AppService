using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands
{
    public class AddNewTeam
    {
        public class Command : IRequest<LeagueDto>
        {
            public string Name { get; set; }
            public string Season { get; set; }
            public int TeamId { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, LeagueDto>
        {
            private readonly ApplicationDbContext _context;
            private readonly ILeagueServiceFactory _leagueServiceFactory;

            public CommandHandler(ApplicationDbContext context, ILeagueServiceFactory leagueServiceFactory)
            {
                _context = context;
                _leagueServiceFactory = leagueServiceFactory;
            }

            public async Task<LeagueDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var league = (await _context.League.SingleOrDefaultAsync(x =>
                        x.Season == request.Season && x.Name == request.Name, cancellationToken))
                    .GuardAgainstNotFound($"No League {request.Name} - ({request.Season}) found");
                var service = _leagueServiceFactory.Create(league.Id);
                await service.AddTeam(request.TeamId, cancellationToken);

                return league.ToLeagueDto();
            }
        }
    }
}