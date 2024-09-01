namespace HomeTownPickEm.Models;

public class WeeklyLeaderboard
{
    public string UserFirstName { get; set; }

    public string UserLastName { get; set; }

    public string TeamSchool { get; set; }

    public string TeamMascot { get; set; }

    public string TeamLogos { get; set; }

    public string LeagueName { get; set; }

    public string LeagueSlug { get; set; }

    public string Year { get; set; }

    public int TotalPoints { get; set; }

    public int TeamId { get; set; }

    public string UserId { get; set; }

    public int LeagueId { get; set; }

    public int Week { get; set; }

    public int SeasonId { get; set; }
}