using System.Collections.Generic;
using HomeTownPickEm.Application.Picks;
using HomeTownPickEm.Application.Teams;
using HomeTownPickEm.Application.Users;

namespace HomeTownPickEm.Application.Leagues
{
    public class LeagueDto
    {
        public LeagueDto()
        {
            Teams = new HashSet<TeamDto>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public string Year { get; set; }

        public IEnumerable<TeamDto> Teams { get; set; }
        public IEnumerable<UserDto> Members { get; set; }
        public IEnumerable<PickDto> Picks { get; set; }
    }
}