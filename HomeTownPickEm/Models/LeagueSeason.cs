using System.Collections.Generic;

namespace HomeTownPickEm.Models
{
    // public static class LeagueSeasonExtenstions
    // {
    //     public static LeagueSeasonDto ToLeagueDto(this LeagueSeason league)
    //     {
    //         return new LeagueSeasonDto
    //         {
    //         };
    //     }
    // }

    public class LeagueSeason
    {
        public LeagueSeason()
        {
            Teams = new HashSet<Team>();
            Members = new HashSet<ApplicationUser>();
            Picks = new HashSet<Pick>();
        }

        public int Id { get; set; }

        public int LeagueId { get; set; }
        public League League { get; set; }

        public string Year { get; set; }

        public ICollection<Team> Teams { get; set; }

        public ICollection<ApplicationUser> Members { get; set; }

        public ICollection<Pick> Picks { get; set; }
    }
}