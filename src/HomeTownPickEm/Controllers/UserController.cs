#region

using System.Net;
using HomeTownPickEm.Application.Users;
using HomeTownPickEm.Application.Users.Commands;
using HomeTownPickEm.Application.Users.Queries;
using HomeTownPickEm.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace HomeTownPickEm.Controllers
{
    public class UserController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<UserDto>> AddUser(AddUser.Command command)
        {
            var user = await Mediator.Send(command);
            return Ok(user);
        }

        [HttpPost("/api/users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> AddUsers(AddUsers.Command command)
        {
            var users = (await Mediator.Send(command)).ToArray();

            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get(string id)
        {
            var user = await Mediator.Send(new GetUser.Query
            {
                Id = id
            });

            return Ok(user);
        }

        [HttpGet("/api/users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await Mediator.Send(new GetAllUsers.Query());

            return Ok(users);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenUserDto>> Login(Login.Query query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("profile")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Profile()
        {
            var id = HttpContext.RequestServices.GetRequiredService<IUserAccessor>().GetCurrentUsername();
            var user = await Mediator.Send(new GetUser.Query
            {
                Id = id
            });

            return Ok(user);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenUserDto>> Register(Register.Command command)
        {
            var user = await Mediator.Send(command);
            return new ObjectResult(user)
            {
                StatusCode = (int)HttpStatusCode.Created
            };
        }

        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPassword.Command command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        [HttpPost("verifyresetpassword")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> VerifyResetPassword(VerifyPasswordReset.Command command)
        {
            var user = await Mediator.Send(command);
            return Ok(user);
        }

        [HttpPost("verifyemail")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> VerifyEmail(VerifyEmail.Command command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
        

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(string id, UpdateUser.Command command)
        {
            command.Id = id;
            var user = await Mediator.Send(command);

            return Ok(user);
        }
    }
}