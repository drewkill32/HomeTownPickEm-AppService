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
            public string Season { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<TeamDto>>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context, GameTeamRepository repository)
            {
                _context = context;
            }

            public async Task<IEnumerable<TeamDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var teamIds = await _context.Season
                    .Where(s => s.LeagueId == request.LeagueId && s.Year == request.Season)
                    .Include(x => x.Teams)
                    .SelectMany(x => x.Teams, (_, team) => team.Id)
                    .ToArrayAsync(cancellationToken);

                var pendingTeamIds = await _context.PendingInvites
                    .Where(p => p.LeagueId == request.LeagueId && p.Season == request.Season && p.TeamId != null)
                    .Select(x => x.TeamId.Value)
                    .ToArrayAsync(cancellationToken);

                var teamIdsToRemove = teamIds.Concat(pendingTeamIds).ToArray();
                var teams = await _context.Teams
                    .Where(x => !teamIdsToRemove.Contains(x.Id))
                    .Where(x => !string.IsNullOrEmpty(x.Conference))
                    .ToArrayAsync(cancellationToken);


                return teams.Select(x => x.ToTeamDto())
                    .OrderBy(x => x.Name)
                    .ToArray();
            }
        }
    }
}