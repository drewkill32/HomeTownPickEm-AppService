using HomeTownPickEm.Application.Leaderboard;
using HomeTownPickEm.Application.Leaderboard.Queries;
using HomeTownPickEm.Application.Leagues;
using HomeTownPickEm.Application.Leagues.Commands;
using HomeTownPickEm.Application.Leagues.Queries;
using HomeTownPickEm.Application.Teams;
using HomeTownPickEm.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeTownPickEm.Controllers
{
    public class LeagueController : ApiControllerBase
    {

        
        [HttpPost]
        public async Task<ActionResult> CreateLeague(AddLeague.Command command)
        {
            var league = await Mediator.Send(command);

            return CreatedAtAction("GetLeague", league);
        }

        [HttpGet("settings/{LeagueId}")]
        public async Task<ActionResult<LeagueSettingsDto>> GetLeagueSettings([FromRoute] GetLeagueSettings.Query query)
        {
            var leagueSettings = await Mediator.Send(query);

            return Ok(leagueSettings);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<UserLeagueDto>>> GetUserLeagues()
        {
            var leagues = await Mediator.Send(new GetUserLeagues.Query());

            return Ok(leagues);
        }

        [HttpPost("new-season")]
        public async Task<IActionResult> CreateSeason(AddSeason.Command command)
        {
            await Mediator.Send(command);
            return new StatusCodeResult(StatusCodes.Status201Created);
        }

        [HttpPost("add-member")]
        public async Task<IActionResult> AddMember(AddNewMember.Command command)
        {
            await Mediator.Send(command);
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        [HttpPost("add-team")]
        public async Task<IActionResult> AddTeam(AddNewTeam.Command command)
        {
            await Mediator.Send(command);
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }


        [HttpPost("remove-team")]
        public async Task<IActionResult> RemoveTeam(RemoveTeam.Command command)
        {
            await Mediator.Send(command);
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        [HttpPost("remove-member")]
        public async Task<IActionResult> RemoveMember(RemoveMember.Command command)
        {
            await Mediator.Send(command);
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        [HttpPost("remove-pending-team")]
        public async Task<IActionResult> RemovePendingTeam(RemovePendingTeam.Command command)
        {
            await Mediator.Send(command);
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        [HttpPost("remove-pending-member")]
        public async Task<IActionResult> RemovePendingMember(RemovePendingMember.Command command)
        {
            await Mediator.Send(command);
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        [HttpPost("make-commissioner")]
        public async Task<IActionResult> MakeCommissioner(MakeCommissioner.Command command)
        {
            await Mediator.Send(command);
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        [HttpPost("remove-commissioner")]
        public async Task<IActionResult> RemoveCommissioner(RemoveCommissioner.Command command)
        {
            await Mediator.Send(command);
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }


        [HttpGet("{LeagueSlug}/{Season}/leaderboard")]
        public async Task<ActionResult<LeaderBoardDto>> Get([FromRoute] GetLeaderboard.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{LeagueSlug}/{Season}", Name = "GetLeague")]
        public async Task<ActionResult<LeagueDto>> GetLeague([FromRoute] GetLeague.Query query)
        {
            var league = await Mediator.Send(query);

            return Ok(league);
        }

        [HttpGet("{LeagueId}/{Season}/game/{GameId}")]
        public async Task<ActionResult<IEnumerable<UserPickResponse>>> GetMemberGamePicks(
            [FromRoute] GetMemberPicksByGame.Query query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{LeagueId}/{Season}/availableteams")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetNotInLeague([FromRoute] TeamsNotInLeague.Query query)
        {
          
            var teams = await Mediator.Send(query);
            return Ok(teams);
        }
        

        [HttpDelete]
        public async Task<ActionResult<IEnumerable<UserLeagueDto>>> DeleteSeason([FromQuery] RemoveSeason.Command query)
        {
            await Mediator.Send(query);
            return Ok();
        }
        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLeagueDto>>> GetLeagues([FromQuery] GetLeagues.Query query)
        {
            var leagues = await Mediator.Send(query);
            return Ok(leagues);
        }

        [HttpGet("{Id}/{Season}/MembersTeams")]
        public async Task<IActionResult> GetLeagueMembersAndTeams([FromRoute] GetLeagueMembersAndTeams.Query query)
        {
            var leagues = await Mediator.Send(query);
            return Ok(leagues);
        }
        
    }
}