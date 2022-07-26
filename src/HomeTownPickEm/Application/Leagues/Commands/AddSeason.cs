using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands;

public class AddSeason
{
    public class Command : IRequest
    {
        public int LeagueId { get; set; }

        public string Year { get; set; }

        public CopyFrom CopyFrom { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;

        public CommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var league = await _context.League.FindAsync(request.LeagueId);
            if (league == null)
            {
                throw new NotFoundException($"League '{request.LeagueId}' not found");
            }


            var newSeason = new Season
            {
                LeagueId = request.LeagueId,
                Year = request.Year,
                Active = true
            };

            if (request.CopyFrom != null)
            {
                var prevSeason = await _context.Season.Where(x =>
                        x.LeagueId == request.LeagueId && x.Year == request.CopyFrom.Season)
                    .Include(x => x.Teams)
                    .Include(x => x.Members)
                    .AsTracking()
                    .FirstOrDefaultAsync(cancellationToken);


                if (request.CopyFrom.Members)
                {
                    foreach (var member in prevSeason.Members)
                    {
                        newSeason.AddMember(member);
                    }
                }

                if (request.CopyFrom.Teams)
                {
                    foreach (var team in prevSeason.Teams)
                    {
                        var games = await _context.Games.Where(x => x.Season == request.Year)
                            .Where(x => x.HomeId == team.Id || x.AwayId == team.Id)
                            .AsTracking()
                            .ToArrayAsync(cancellationToken);

                        newSeason.AddTeam(team, games);
                    }
                }
            }

            _context.Add(newSeason);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}

public class CopyFrom
{
    public string Season { get; set; }
    public bool Teams { get; set; }
    public bool Members { get; set; }
}