using System.Collections.Generic;
using System.Linq;
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
            return new LeagueDto
            {
                Id = league.Id,
                Name = league.Name,
                Year = league.Season,
                Teams = league.Teams?.Select(x => x.ToTeamDto()),
                Members = league.Members?.Select(x => x.ToUserDto()),
                Picks = league.Picks?.Select(x => x.ToPickDto())
            };
        }
    }

    public class League
    {
        public League()
        {
            Teams = new HashSet<Team>(ModelEquality<Team>.IdComparer);
            Members = new HashSet<ApplicationUser>(ModelEquality<ApplicationUser>.IdComparer);
            Picks = new HashSet<Pick>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Season { get; set; }

        public ICollection<Team> Teams { get; set; }

        public ICollection<ApplicationUser> Members { get; set; }

        public ICollection<Pick> Picks { get; set; }
    }
}