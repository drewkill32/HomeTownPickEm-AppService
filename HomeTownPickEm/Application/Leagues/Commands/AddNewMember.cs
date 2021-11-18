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
    public class AddNewMemberCommandHandler : IRequestHandler<AddNewMemberCommand, LeagueDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILeagueServiceFactory _leagueServiceFactory;

        public AddNewMemberCommandHandler(ApplicationDbContext context, ILeagueServiceFactory leagueServiceFactory)
        {
            _context = context;
            _leagueServiceFactory = leagueServiceFactory;
        }

        public async Task<LeagueDto> Handle(AddNewMemberCommand request, CancellationToken cancellationToken)
        {
            var league = (await _context.League.SingleOrDefaultAsync(x =>
                    x.Season == request.Season && x.Name == request.Name, cancellationToken))
                .GuardAgainstNotFound($"No League {request.Name} - ({request.Season}) found");
            var service = _leagueServiceFactory.Create(league.Id);
            await service.AddUserAsync(request.UserId, cancellationToken);
            return league.ToLeagueDto();
        }
    }

    public class AddNewMemberCommand : IRequest<LeagueDto>
    {
        public string Name { get; set; }
        public string Season { get; set; }
        public string UserId { get; set; }
    }
}