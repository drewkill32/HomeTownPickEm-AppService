using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands
{
    public class AddLeagueTeam
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

            public CommandHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<LeagueDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var team = await _context.Teams.SingleOrDefaultAsync(x => x.Id == request.TeamId, cancellationToken);
                if (team == null)
                {
                    throw new NotFoundException($"No team found with Id: '{request.TeamId}'");
                }

                var league =
                    await _context.League.Where(x => x.Name == request.Name && x.Season == request.Season)
                        .Include(x => x.Teams)
                        .SingleOrDefaultAsync(cancellationToken);

                if (league == null)
                {
                    throw new NotFoundException(
                        $"No League found with name {request.Name} and Season {request.Season}");
                }

                if (league.Teams.Any(x => x.Id == request.TeamId))
                {
                    throw new BadRequestException(
                        $"The team '{team.School} {team.Mascot}' Id: {request.TeamId} is already associated with league {request.Name}-{request.Season}");
                }

                league.Teams.Add(team);

                _context.League.Update(league);

                await _context.SaveChangesAsync(cancellationToken);

                return league.ToLeagueDto();
            }
        }
    }
}