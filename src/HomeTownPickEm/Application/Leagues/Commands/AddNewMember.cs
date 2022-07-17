using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands
{
    public class AddNewMember
    {
        public class Command : IRequest<LeagueDto>
        {
            public string LeagueSlug { get; set; }
            public string Season { get; set; }
            public string UserId { get; set; }
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
                var season = (await _context.Season.SingleOrDefaultAsync(x =>
                        x.Year == request.Season && x.League.Slug == request.LeagueSlug, cancellationToken))
                    .GuardAgainstNotFound($"No League {request.LeagueSlug} - ({request.Season}) found");
                var service = _leagueServiceFactory.Create(season.Id);
                await service.AddUserAsync(request.UserId, cancellationToken);
                return season.ToLeagueDto();
            }
        }
    }
}