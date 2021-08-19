using System.Collections.Generic;
using System.Linq;
using HomeTownPickEm.Application.Games;
using HomeTownPickEm.Application.Teams;
using HomeTownPickEm.Models;

namespace HomeTownPickEm.Application.Picks
{
    public static class PickExtensions
    {
        public static PickDto ToPickDto(this Pick pick)
        {
            return new PickDto
            {
                Id = pick.Id,
                Points = pick.Points,
                LeagueId = pick.LeagueId,
                GameId = pick.GameId,
                Game = pick.Game.ToGameDto(),
                TeamsPicked = pick.TeamsPicked.Select(x => x.ToTeamDto()).ToArray(),
                UserId = pick.UserId
            };
        }
    }

    public class PickDto
    {
        public int Id { get; set; }

        public int LeagueId { get; set; }

        public int GameId { get; set; }

        public GameDto Game { get; set; }

        public string UserId { get; set; }


        public int Points { get; set; }

        public ICollection<TeamDto> TeamsPicked { get; set; }
    }
}