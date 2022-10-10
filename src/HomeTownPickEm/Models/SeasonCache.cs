namespace HomeTownPickEm.Models;

public class SeasonCache
{
    public DateTimeOffset LastRefresh { get; set; }

    public string Season { get; set; }

    public int Week { get; set; }

    public string Type { get; set; }
}