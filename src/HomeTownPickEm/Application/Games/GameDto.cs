using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Application.Teams;
using HomeTownPickEm.Models;

namespace HomeTownPickEm.Application.Games
{
    public static class GameExtensions
    {
        public static GameDto ToGameDto(this Game game)
        {
            return new GameDto
            {
                Id = game.Id,
                Week = game.Week,
                Season = game.Season,
                HomeTeam = game.Home?.ToTeamDto(),
                AwayTeam = game.Away?.ToTeamDto(),
                AwayPoints = game.AwayPoints,
                HomePoints = game.HomePoints,
                StartTimeTbd = game.StartTimeTbd,
                SeasonType = game.SeasonType,
                StartDate = game.StartDate
            };
        }
    }

    public class GameDto : IMapFrom<Game>
    {
        public int Id { get; set; }

        public string Season { get; set; }

        public int Week { get; set; }

        public string SeasonType { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public bool StartTimeTbd { get; set; }

        public int? HomePoints { get; set; }

        public TeamDto HomeTeam { get; set; }
        public TeamDto AwayTeam { get; set; }

        public int? AwayPoints { get; set; }


        public bool GameFinal { get; set; }

        public TeamDto Winner
        {
            get
            {
                if (!GameFinal)
                {
                    return null;
                }

                return HomePoints > AwayPoints ? HomeTeam : AwayTeam;
            }
        }
    }
}