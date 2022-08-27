using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers;

public class AppController : ApiControllerBase
{
    [AllowAnonymous]
    [HttpGet("/heartbeat")]
    public IActionResult Heartbeat()
    {
        return Ok("OK");
    }
}