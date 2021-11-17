using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Teams;
using HomeTownPickEm.Data;
using HomeTownPickEm.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Queries
{
    public class TeamsNotInLeagueQueryHandler : IRequestHandler<TeamsNotInLeagueQuery, IEnumerable<TeamDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly GameTeamRepository _repository;

        public TeamsNotInLeagueQueryHandler(ApplicationDbContext context, GameTeamRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<IEnumerable<TeamDto>> Handle(TeamsNotInLeagueQuery request,
            CancellationToken cancellationToken)
        {
            var teamIds = await _context.League
                .Include(x => x.Teams)
                .SelectMany(x => x.Teams, (_, team) => team.Id)
                .ToArrayAsync(cancellationToken);

            var teams = await _context.Teams
                .Where(x => !teamIds.Contains(x.Id))
                .Where(x => !string.IsNullOrEmpty(x.Conference))
                .ToArrayAsync(cancellationToken);


            return teams.Select(x => x.ToTeamDto())
                .OrderBy(x => x.Name)
                .ToArray();
        }
    }

    public class TeamsNotInLeagueQuery : IRequest<IEnumerable<TeamDto>>
    {
        public int LeagueId { get; set; }
    }
}