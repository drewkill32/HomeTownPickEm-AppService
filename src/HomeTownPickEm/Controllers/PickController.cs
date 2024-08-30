using System.Threading.Tasks;
using HomeTownPickEm.Application.Leagues.Queries;
using HomeTownPickEm.Application.Picks;
using HomeTownPickEm.Application.Picks.Commands;
using HomeTownPickEm.Application.Picks.Queries;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers;

public class PickController : ApiControllerBase
{
    [HttpPut]
    public async Task<ActionResult<PickDto>> CreatePick(SelectTeam.Command command)
    {
        var pick = await Mediator.Send(command);
        return Ok(pick);
    }

    [HttpPost("tiebreaker-pick")]
    public async Task<ActionResult> UpsertTiebreakerPick(SelectTiebreaker.Command command)
    {
        await Mediator.Send(command);
        return Ok();
    }

    [HttpGet("tiebreaker-pick/{WeeklyGameId}")]
    public async Task<ActionResult<int?>> GetTiebreaker([FromRoute] GetTieBreakerPick.Query query)
    {
        var result = await Mediator.Send(query);
        return Ok(new
        {
            TotalPoints = result
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PickDto>> GetPick(int id)
    {
        var pick = await Mediator.Send(new GetPick.Query
        {
            Id = id
        });
        return Ok(pick);
    }

    [HttpPost("admin/update/pick")]
    public async Task<ActionResult<PickDto>> AdminUpdatePick(AdminCreatePick.Command command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
}