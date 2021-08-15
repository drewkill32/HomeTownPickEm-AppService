using System;
using HomeTownPickEm.Application.Teams.Commands;

namespace HomeTownPickEm.Application.Games
{
    public class GameDto
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


        public bool GameFinal => HomePoints.HasValue && AwayPoints.HasValue;


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