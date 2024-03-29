using HomeTownPickEm.Application.Common;

namespace HomeTownPickEm.Models
{
    public class Game : IHasId
    {
        public int Id { get; set; }

        public string Season { get; set; }

        public int Week { get; set; }

        public string SeasonType { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public bool StartTimeTbd { get; set; }

        public int HomeId { get; set; }

        public int? HomePoints { get; set; }

        public int AwayId { get; set; }

        public Team Home { get; set; }

        public Team Away { get; set; }

        public int? AwayPoints { get; set; }
        public ICollection<Pick> Picks { get; set; }

        public bool GameFinal => HomePoints.HasValue && AwayPoints.HasValue;


        public string Winner
        {
            get
            {
                if (!GameFinal)
                {
                    return "Pending";
                }

                return HomePoints > AwayPoints ? nameof(Home) : nameof(Away);
            }
        }

        public int? WinnerId
        {
            get
            {
                if (!GameFinal)
                {
                    return null;
                }

                return HomePoints > AwayPoints ? HomeId : AwayId;
            }
        }

        public bool TeamIsPlaying(int teamId)
        {
            return HomeId == teamId || AwayId == teamId;
        }

        public bool TeamIsPlaying(Team team)
        {
            return TeamIsPlaying(team.Id);
        }

        public override string ToString()
        {
            return $"{Away} at {Home}";
        }
    }
}