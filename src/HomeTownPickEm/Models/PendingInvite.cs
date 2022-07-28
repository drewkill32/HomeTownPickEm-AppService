namespace HomeTownPickEm.Models;

public class PendingInvite
{
    public int LeagueId { get; set; }
    public string Season { get; set; }
    public string UserId { get; set; }
    public int? TeamId { get; set; }
}