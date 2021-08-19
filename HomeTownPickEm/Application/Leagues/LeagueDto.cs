using System.Collections.Generic;
using HomeTownPickEm.Application.Teams;

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

        public ICollection<TeamDto> Teams { get; set; }
    }
}