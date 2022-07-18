using HomeTownPickEm.Application.Leagues;
using HomeTownPickEm.Application.Picks;
using HomeTownPickEm.Application.Teams;
using HomeTownPickEm.Application.Users;

namespace HomeTownPickEm.Models;

public static class SeasonExtenstions
{
    public static LeagueDto ToLeagueDto(this Season season)
    {
        return new LeagueDto
        {
            Id = season.Id,
            Name = season.League.Name,
            Year = season.Year,
            Teams = season.Teams?.Select(x => x.ToTeamDto()),
            Members = season.Members?.Select(x => x.ToUserDto()),
            Picks = season.Picks?.Select(x => x.ToPickDto())
        };
    }
}

public class Season
{
    public Season()
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

    public string GetLeagueName()
    {
        return League?.Slug + ":" + Year;
    }
    
}