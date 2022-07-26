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

    public bool Active { get; set; }

    public ICollection<Team> Teams { get; set; }

    public ICollection<ApplicationUser> Members { get; set; }

    public ICollection<Pick> Picks { get; set; }


    public void RemoveTeam(Team team)
    {
        Teams.Remove(team);
        var picks = Picks.Where(p => p.ContainsTeam(team)).ToArray();
        var h2hPickRemoved = new HashSet<int>();
        foreach (var pick in picks)
        {
            if (IsHead2Head(pick.Game) && !h2hPickRemoved.Contains(pick.GameId))
            {
                Picks.Remove(pick);
                h2hPickRemoved.Add(pick.GameId);
            }
            else
            {
                Picks.Remove(pick);
            }
        }
    }

    public void RemoveMember(ApplicationUser user)
    {
        Members.Remove(user);
        var picks = Picks.Where(x => x.UserId == user.Id).ToArray();
        foreach (var pick in picks)
        {
            Picks.Remove(pick);
        }
    }


    public void AddTeam(Team team, IEnumerable<Game> games)
    {
        if (Teams.Any(x => x.Id == team.Id))
        {
            throw new InvalidOperationException($"Team {team.Id} already exists in season {Year}");
        }

        Teams.Add(team);

        foreach (var member in Members)
        {
            var newPicks = games.Select(x => new Pick
            {
                Points = 0,
                GameId = x.Id,
                Game = x,
                UserId = member.Id
            }).ToArray();

            foreach (var newPick in newPicks)
            {
                var picks = Picks
                    .Where(x => x.UserId == member.Id && x.GameId == newPick.GameId)
                    .ToArray();

                if (picks.Length == 0)
                {
                    Picks.Add(newPick);
                }

                var isHead2Head = IsHead2Head(newPick.Game);

                //add another pick 
                if (isHead2Head && picks.Length < 2)
                {
                    Picks.Add(new Pick
                    {
                        Points = 0,
                        GameId = newPick.Id,
                        Game = newPick.Game,
                        UserId = newPick.UserId
                    });
                }
            }
        }
    }

    public void AddMember(ApplicationUser user)
    {
        if (Members.Any(x => x.Id == user.Id))
        {
            throw new InvalidOperationException($"User {user.Id} already exists in season {Year}");
        }

        Members.Add(user);
    }

    public bool IsHead2Head(Game game)
    {
        var teamIds = Teams.Select(x => x.Id).ToArray();
        return teamIds.Contains(game.AwayId) && teamIds.Contains(game.HomeId);
    }
    
    
}