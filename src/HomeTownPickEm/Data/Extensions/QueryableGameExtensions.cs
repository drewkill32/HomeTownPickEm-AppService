using HomeTownPickEm.Models;

namespace HomeTownPickEm.Data.Extensions
{
    public static class QueryableGameExtensions
    {
        public static IQueryable<Game> WhereTeamIsPlaying(this IQueryable<Game> query, int? teamId)
        {
            if (teamId.HasValue)
            {
                query = query.Where(x => x.HomeId == teamId || x.AwayId == teamId);
            }

            return query;
        }

        public static IQueryable<Game> WhereTeamIsPlaying(this IQueryable<Game> query, Team team)
        {
            return query.WhereTeamIsPlaying(team.Id);
            
        }

        public static IQueryable<Game> WhereTeamsArePlaying(this IQueryable<Game> query, IEnumerable<int> teamIds)
        {
            return query.Where(g => teamIds.Contains(g.HomeId) || teamIds.Contains(g.AwayId));
        }

        public static IQueryable<Game> WhereTeamsArePlaying(this IQueryable<Game> query, IEnumerable<Team> teams)
        {
            var teamIds = teams.Select(x => x.Id).ToArray();
            return query.WhereTeamsArePlaying(teamIds);
        }
        

        public static IQueryable<Game> WhereWeekIs(this IQueryable<Game> query, int? week)
        {
            if (week.HasValue)
            {
                query = query.Where(x => x.Week == week);
            }

            return query;
        }
    }
}