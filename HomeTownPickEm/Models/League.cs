using System.Collections.Generic;
using System.Linq;
using HomeTownPickEm.Application.Leagues;

namespace HomeTownPickEm.Models
{
    public static class LeagueExtenstions
    {
        public static LeagueDto ToLeagueDto(this League league)
        {
            return new LeagueDto
            {
                Id = league.Id,
                Name = league.Name
                //LeagueSeasons = league.LeagueSeasons.Select(ToLeagueSeasonDto);
            };
        }

        public static LeagueDto ToLeagueDto(this League league, string year)
        {
            var season = league.LeagueSeasons.FirstOrDefault(x => x.Year == year);

            return new LeagueDto
            {
                Id = league.Id,
                Name = league.Name,
                Year = season?.Year,
                Teams = season?.Teams.Select(x => x.ToTeamDto()).ToArray()
            };
        }
    }

    public class League
    {
        public League()
        {
            LeagueSeasons = new HashSet<LeagueSeason>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<LeagueSeason> LeagueSeasons { get; set; }
    }
}