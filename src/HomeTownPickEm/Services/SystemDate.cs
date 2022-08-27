using HomeTownPickEm.Application.Common;

namespace HomeTownPickEm.Services;

public class SystemDate : ISystemDate
{
    public DateTimeOffset Now => DateTimeOffset.Now;
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    public string Year => DateTimeOffset.Now.Year.ToString();
}