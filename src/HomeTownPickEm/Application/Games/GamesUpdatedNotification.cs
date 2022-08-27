using MediatR;

namespace HomeTownPickEm.Application.Games;

public class GamesUpdatedNotification : INotification
{
    public string Year { get; set; } = DateTime.Now.Year.ToString();

    public int? Week { get; set; }
}