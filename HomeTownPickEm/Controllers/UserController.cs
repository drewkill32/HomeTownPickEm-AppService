#region

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Users;
using HomeTownPickEm.Application.Users.Commands;
using HomeTownPickEm.Application.Users.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace HomeTownPickEm.Controllers
{
    public class UserController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<UserDto>> AddUser(AddUserCommand command)
        {
            var user = await Mediator.Send(command);
            return Ok(user);
        }

        [HttpPost("/api/users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> AddUsers(AddUsersCommand command)
        {
            var users = (await Mediator.Send(command)).ToArray();

            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get(string id)
        {
            var users = await Mediator.Send(new GetUserQuery
            {
                Id = id
            });

            return Ok(users);
        }

        [HttpGet("/api/users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await Mediator.Send(new GetAllUsersQuery());

            return Ok(users);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Login(LoginQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Register(RegisterCommand command)
        {
            command.LeagueIds = new[] { 1 };
            var user = await Mediator.Send(command);
            return new ObjectResult(user)
            {
                StatusCode = (int)HttpStatusCode.Created
            };
        }

        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(string id, UpdateUserCommand command)
        {
            command.Id = id;
            var users = await Mediator.Send(command);

            return Ok(users);
        }
    }
}