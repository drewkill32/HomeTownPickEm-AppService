using HomeTownPickEm.Application.Teams;
using HomeTownPickEm.Data;
using HomeTownPickEm.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Queries
{
    public class TeamsNotInLeague
    {
        public class Query : IRequest<IEnumerable<TeamDto>>
        {
            public int LeagueId { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<TeamDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly GameTeamRepository _repository;

            public QueryHandler(ApplicationDbContext context, GameTeamRepository repository)
            {
                _context = context;
                _repository = repository;
            }

            public async Task<IEnumerable<TeamDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var teamIds = await _context.Season
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
    }
}