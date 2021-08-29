using HomeTownPickEm.Utils;

namespace HomeTownPickEm.Application.Leaderboard
{
    public static class LeaderBoardDtoExtensions
    {
        public static LeaderBoardDto ToLeaderBoardDto(this Models.Leaderboard l)
        {
            return new LeaderBoardDto
            {
                User = $"{l.UserFirstName} {l.UserLastName}",
                Team = $"{l.TeamSchool} {l.TeamMascot}",
                TeamLogo = LogoHelper.GetSingleLogo(l.TeamLogos),
                LeagueName = l.LeagueName,
                TotalPoints = l.TotalPoints
            };
        }
    }

    public class LeaderBoardDto
    {
        public string User { get; set; }

        public string Team { get; set; }

        public string TeamLogo { get; set; }

        public string LeagueName { get; set; }

        public int TotalPoints { get; set; }
    }
}