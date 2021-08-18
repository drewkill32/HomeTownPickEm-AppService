using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Users;
using HomeTownPickEm.Application.Users.Commands;
using HomeTownPickEm.Application.Users.Queries;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get(string id)
        {
            var users = await Mediator.Send(new GetUser.Query
            {
                Id = id
            });

            return Ok(users);
        }

        [HttpGet("/api/users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await Mediator.Send(new GetAllUsers.Query());

            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(string id, UpdateUser.Command command)
        {
            command.Id = id;
            var users = await Mediator.Send(command);

            return Ok(users);
        }
    }
}