namespace HomeTownPickEm.Models;


public class Season
{
    private readonly HashSet<Team> _teams;
    private readonly HashSet<Pick> _picks;
    private readonly HashSet<ApplicationUser> _members;

    public Season()
    {
        _teams = new HashSet<Team>();
        _members = new HashSet<ApplicationUser>();
        _picks = new HashSet<Pick>();
    }

    public int Id { get; set; }

    public int LeagueId { get; set; }

    public League League { get; set; }

    public string Year { get; set; }

    public bool Active { get; set; }

    public IReadOnlyCollection<Team> Teams => _teams;

    public IReadOnlyCollection<ApplicationUser> Members => _members;

    public IReadOnlyCollection<Pick> Picks => _picks;


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
        
        foreach (var member in Members)
        {
            AssignPicks(member, games);
        }
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

        AssignPicks(member, games);
    }

    private void AssignPicks(ApplicationUser member, IEnumerable<Game> games)
    {
        var newPicks = games
            .Where(g => g.Season == Year)
            .Select(x => new Pick
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
                _picks.Add(newPick);
            }

            var isHead2Head = IsHead2Head(newPick.Game);

            //add another pick 
            if (isHead2Head && picks.Length < 2)
            {
                _picks.Add(new Pick
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