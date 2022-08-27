namespace HomeTownPickEm.Application.Seasons;

public class SeasonDto
{
    public string Season { get; set; }
    public DateTimeOffset FirstGameStart { get; set; }
    public DateTimeOffset LastGameStart { get; set; }
}