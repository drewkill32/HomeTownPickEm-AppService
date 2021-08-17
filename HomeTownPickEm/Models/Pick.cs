using System.Collections.Generic;

namespace HomeTownPickEm.Models
{
    public class Pick
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }

        public League League { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int Points { get; set; }


        public ICollection<Team> TeamsPicked { get; set; }
    }
}