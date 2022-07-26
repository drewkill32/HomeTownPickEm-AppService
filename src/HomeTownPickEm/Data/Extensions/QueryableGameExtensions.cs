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
            if (team != null)
            {
                var teamId = team.Id;
                query = query.Where(x => x.HomeId == teamId || x.AwayId == teamId);
            }

            return query;
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