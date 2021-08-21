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
            var pickDto = new PickDto
            {
                Id = pick.Id,
                Points = pick.Points,
                LeagueId = pick.LeagueId,
                GameId = pick.GameId,
                Game = pick.Game?.ToGameDto(),
                SelectedTeam = pick.SelectedTeam?.ToTeamDto(),
                UserId = pick.UserId
            };
            if (pick.League != null && pick.Game != null)
            {
                var game = pick.Game;
                var leagueTeams = pick.League.Teams;
                if (leagueTeams.Any(x => x.Id == game.AwayId) && leagueTeams.Any(x => x.Id == game.HomeId))
                {
                    pickDto.Head2Head = true;
                }
            }

            return pickDto;
        }
    }

    public class PickDto
    {
        public int Id { get; set; }

        public int LeagueId { get; set; }

        public int GameId { get; set; }

        public GameDto Game { get; set; }

        public string UserId { get; set; }

        public bool Head2Head { get; set; }
        public int Points { get; set; }

        public TeamDto SelectedTeam { get; set; }
    }
}