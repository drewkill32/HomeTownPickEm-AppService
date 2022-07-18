using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers;

public class DevController : ApiControllerBase
{
    [HttpPost]
    [Route("setDate")]
    public IActionResult SetDate([FromBody] SetDateModel model)
    {
        if (HttpContext.RequestServices.GetRequiredService<ISystemDate>() is not DevSystemDate devDateTime)
        {
            return BadRequest("There is no DevSystemDate service set up");
        }

        var date = new DateTimeOffset(DateTime.SpecifyKind(model.Date, DateTimeKind.Local));
        devDateTime.SetNow(date);
        return Ok(new
        {
            date = date.ToString()
        });
    }
}

public class SetDateModel
{
    public DateTime Date { get; set; }
}