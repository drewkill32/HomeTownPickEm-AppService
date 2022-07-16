using System;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Models;

namespace HomeTownPickEm.Services.Cfbd
{
    public static class GameResponseExtensions
    {
        public static Game ToGame(this GameResponse gameResponse)
        {
            return new Game
            {
                Id = gameResponse.Id,
                Season = gameResponse.Season.ToString(),
                Week = gameResponse.Week,
                AwayId = gameResponse.AwayId,
                AwayPoints = gameResponse.AwayPoints,
                HomeId = gameResponse.HomeId,
                HomePoints = gameResponse.HomePoints,
                SeasonType = gameResponse.SeasonType,
                StartTimeTbd = gameResponse.StartTimeTbd,
                StartDate = gameResponse.StartDate
            };
        }
    }

    public class GameResponse: IHasId
    {
        public int Id { get; set; }

        public int Season { get; set; }

        public int Week { get; set; }

        public string SeasonType { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public bool StartTimeTbd { get; set; }

        public int HomeId { get; set; }

        public int? HomePoints { get; set; }

        public int AwayId { get; set; }

        public int? AwayPoints { get; set; }

        public bool GameFinal => HomePoints.HasValue && AwayPoints.HasValue;


        public int? Winner
        {
            get
            {
                if (!GameFinal)
                {
                    return null;
                }

                return HomePoints > AwayPoints ? HomeId : AwayId;
            }
        }
    }
}