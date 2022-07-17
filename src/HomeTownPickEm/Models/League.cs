using HomeTownPickEm.Application.Leagues;
using HomeTownPickEm.Application.Picks;
using HomeTownPickEm.Application.Teams;
using HomeTownPickEm.Application.Users;

namespace HomeTownPickEm.Models
{
    public static class LeagueExtenstions
    {
        public static LeagueDto ToLeagueDto(this League league)
        {
            var season = league.Seasons.OrderBy(x => x.Year).Last();
            return new LeagueDto
            {
                Id = league.Id,
                Name = league.Name,
                Year = season.Year,
                Teams = season.Teams?.Select(x => x.ToTeamDto()),
                Members = season.Members?.Select(x => x.ToUserDto()),
                Picks = season.Picks?.Select(x => x.ToPickDto())
            };
        }
    }

    public class League
    {
        public League()
        {
            Seasons = new HashSet<Season>();
        }


        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public ICollection<Season> Seasons { get; set; }
        
    }
}