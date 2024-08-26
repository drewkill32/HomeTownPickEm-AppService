namespace HomeTownPickEm.Models;

public class WeeklyGame
{
    private HashSet<WeeklyGamePick> _weeklyGamePicks;

    public WeeklyGame()
    {
        _weeklyGamePicks = new();
    }

    public Guid Id { get; set; }

    public int SeasonId { get; set; }

    public Season Season { get; set; }

    public int GameId { get; set; }

    public int Week { get; set; }

    public IReadOnlyCollection<WeeklyGamePick> WeeklyGamePicks
    {
        get => _weeklyGamePicks;
        set => _weeklyGamePicks = value.ToHashSet();
    }
}