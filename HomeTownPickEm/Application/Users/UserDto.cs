using System.Collections.Generic;
using System.Linq;
using HomeTownPickEm.Application.Teams;
using HomeTownPickEm.Models;

namespace HomeTownPickEm.Application.Users
{
    public static class UserExtensions
    {
        public static UserDto ToUserDto(this ApplicationUser user, string token = "")
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.Name.First,
                LastName = user.Name.Last,
                Team = user.Team?.ToTeamDto() ?? new TeamDto(),
                Leagues = user.Leagues.Select(x => $"{x.Name} - ({x.Season})"),
                Token = token
            };
        }
    }

    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public TeamDto Team { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public IEnumerable<string> Leagues { get; set; }

        public string Token { get; set; }
    }
}