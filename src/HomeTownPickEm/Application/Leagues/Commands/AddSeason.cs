using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Commands;

public class AddSeason
{
    public class Command : ILeagueCommissionerRequest
    {
        public int LeagueId { get; set; }

        public string Year { get; set; }

        public CopyFrom CopyFrom { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpContext _httpContext;

        public CommandHandler(ApplicationDbContext context,
            IHttpContextAccessor accessor)
        {
            _context = context;
            _httpContext = accessor.HttpContext;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var league = await GetLeague(request);

            GuardAgainstSeasonExists(request, league);


            var newSeason = new Season
            {
                LeagueId = request.LeagueId,
                Year = request.Year,
                Active = true
            };

            if (request.CopyFrom != null)
            {
                await CopyFromPrevSeason(request, newSeason, cancellationToken);
            }

            var oldSeasons = await _context.Season
                .AsTracking().
                ToArrayAsync(cancellationToken);

            foreach (var oldSeason in oldSeasons)
            {
                oldSeason.Active = false;
                _context.Season.Update(oldSeason);
            }
            _context.Add(newSeason);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        private async Task CopyFromPrevSeason(Command request, Season newSeason, CancellationToken cancellationToken)
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
                        .WhereTeamIsPlaying(team)
                        .AsTracking()
                        .ToArrayAsync(cancellationToken);

                    newSeason.AddTeam(team, games);
                }
            }
        }

        private void GuardAgainstSeasonExists(Command request, League league)
        {
            if (_context.Season.Any(s => s.Year == request.Year && s.LeagueId == request.LeagueId))
            {
                throw new BadRequestException($"The season {request.Year} already exists for {league.Name}");
            }
        }

        private async Task<League> GetLeague(Command request)
        {
            var league = await _context.League.FindAsync(request.LeagueId);
            if (league == null)
            {
                throw new NotFoundException($"League '{request.LeagueId}' not found");
            }

            return league;
        }
        


    }
}

public class CopyFrom
{
    public string Season { get; set; }
    public bool Teams { get; set; }
    public bool Members { get; set; }
}