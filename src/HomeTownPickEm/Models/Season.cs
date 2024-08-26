namespace HomeTownPickEm.Models;

public class Season
{
    private HashSet<Team> _teams;
    private HashSet<Pick> _picks;
    private HashSet<ApplicationUser> _members;
    private HashSet<WeeklyGame> _weeklyGames;

    public Season()
    {
        _teams = new();
        _members = new();
        _picks = new();
        _weeklyGames = new();
    }

    public int Id { get; set; }

    public int LeagueId { get; set; }

    public League League { get; set; }

    public string Year { get; set; }

    public bool Active { get; set; }

    public IReadOnlyCollection<Team> Teams
    {
        get => _teams;
        set => _teams = value.ToHashSet();
    }

    public IReadOnlyCollection<ApplicationUser> Members
    {
        get => _members;
        set => _members = value.ToHashSet();
    }

    public IReadOnlyCollection<Pick> Picks
    {
        get => _picks;
        set => _picks = value.ToHashSet();
    }

    public IReadOnlyCollection<WeeklyGame> WeeklyGames
    {
        get => _weeklyGames;
        set => _weeklyGames = value.ToHashSet();
    }


    public void RemoveTeam(Team team)
    {
        var picks = Picks.Where(p => p.Game.TeamIsPlaying(team)).ToArray();
        var h2hPickRemoved = new HashSet<int>();
        foreach (var pick in picks)
        {
            if (IsHead2Head(pick.Game))
            {
                if (!h2hPickRemoved.Contains(pick.GameId))
                {
                    _picks.Remove(pick);
                    h2hPickRemoved.Add(pick.GameId);
                }
            }
            else
            {
                _picks.Remove(pick);
            }
        }

        _teams.Remove(team);
    }

    public void RemoveMember(ApplicationUser user)
    {
        _members.Remove(user);
        var picks = Picks.Where(x => x.UserId == user.Id).ToArray();
        foreach (var pick in picks)
        {
            _picks.Remove(pick);
        }
    }


    public void AddTeam(Team team)
    {
        if (_teams.Any(x => x.Id == team.Id))
        {
            throw new InvalidOperationException($"Team {team.Id} already exists in season {Year}");
        }

        _teams.Add(team);
    }

    public void AddTeam(Team team, IEnumerable<Game> games)
    {
        AddTeam(team);
    }

    public void AddMember(ApplicationUser user)
    {
        if (_members.Any(x => x.Id == user.Id))
        {
            throw new InvalidOperationException($"User {user.Id} already exists in season {Year}");
        }

        _members.Add(user);
    }

    public bool IsHead2Head(Game game)
    {
        var teamIds = Teams.Select(x => x.Id).ToArray();
        return teamIds.Contains(game.AwayId) && teamIds.Contains(game.HomeId);
    }

    public void UpdatePicks(Game[] games)
    {
        var gamesDict = games.ToHashSet(ModelEquality<Game>.IdComparer).ToDictionary(x => x.Id, x => x);
        var picks = Picks.Where(p => gamesDict.Keys.Contains(p.GameId)).ToArray();
        foreach (var pick in picks)
        {
            var game = gamesDict[pick.GameId];
            if (game.GameFinal && game.WinnerId == pick.SelectedTeamId)
            {
                pick.Points = 1;
            }
        }
    }

    public void AddMember(ApplicationUser member, IEnumerable<Game> games)
    {
        AddMember(member);
    }
}