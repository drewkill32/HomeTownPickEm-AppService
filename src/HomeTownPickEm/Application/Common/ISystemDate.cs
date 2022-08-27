namespace HomeTownPickEm.Application.Common;

public interface ISystemDate
{
    DateTimeOffset Now { get; }
    DateTimeOffset UtcNow { get; }

    string Year { get; }
}