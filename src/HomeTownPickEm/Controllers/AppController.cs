using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers;

public class AppController : ApiControllerBase
{
    private readonly ILogger<AppController> _logger;

    public AppController(ILogger<AppController> logger)
    {
        _logger = logger;
    }
    
    [AllowAnonymous]
    [HttpGet("/heartbeat")]
    public IActionResult Heartbeat()
    {
        _logger.LogInformation("Heartbeat");
        return Ok("OK");
    }
}