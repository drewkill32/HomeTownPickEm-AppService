using Microsoft.AspNetCore.SignalR;

namespace HomeTownPickEm.Hubs;

public class CacheHub : Hub<ICacheClient>
{
    private readonly ILogger<CacheHub> _logger;

    public CacheHub(ILogger<CacheHub> logger)
    {
        _logger = logger;
    }

    public async Task SendMessage()
    {
        await Clients.All.RefreshCache();
    }

    public override Task OnConnectedAsync()
    {
        _logger.LogInformation("Connected");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        _logger.LogInformation("Disconnected");
        return base.OnDisconnectedAsync(exception);
    }
}

public interface ICacheClient
{
    Task RefreshCache();
}