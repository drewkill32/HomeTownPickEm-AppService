namespace HomeTownPickEm.Models
{
    public class Pick
    {
        public int Id { get; set; }
        public int SeasonId { get; set; }

        public Season Season { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int Points { get; set; }


        public Team SelectedTeam { get; set; }

        public int? SelectedTeamId { get; set; }
        
        public bool ContainsTeam(Team team)
        {
            return Game.HomeId == team.Id || Game.AwayId == team.Id;
        }
    }
}