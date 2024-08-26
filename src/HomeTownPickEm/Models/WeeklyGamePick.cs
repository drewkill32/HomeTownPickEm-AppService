namespace HomeTownPickEm.Models;

public class WeeklyGamePick
{
    public Guid Id { get; set; }

    public Guid WeeklyGameId { get; set; }

    public WeeklyGame WeeklyGame { get; set; }

    public int GameId { get; set; }

    public string UserId { get; set; }

    public ApplicationUser User { get; set; }

    public int TotalPoints { get; set; }
}