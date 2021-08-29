using HomeTownPickEm.Models;
using HomeTownPickEm.Utils;

namespace HomeTownPickEm.Application.Teams
{
    public static class TeamExtensions
    {
        public static TeamDto ToTeamDto(this Team team)
        {
            TeamDto teamDto = new()
            {
                Abbreviation = team.Abbreviation,
                Color = team.Color,
                Conference = team.Conference,
                Division = team.Division,
                Mascot = team.Mascot,
                Id = team.Id,
                Logo = LogoHelper.GetSingleLogo(team.Logos),
                School = team.School,
                AltColor = team.AltColor
            };
            return teamDto;
        }
    }

    public class TeamDto
    {
        public int Id { get; set; }
        public string School { get; set; }
        public string Mascot { get; set; }
        public string Abbreviation { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
        public string Color { get; set; }
        public string AltColor { get; set; }
        public string Logo { get; set; }

        public string Name => $"{School} {Mascot}";
    }
}