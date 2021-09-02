using System;

namespace HomeTownPickEm.Models
{
    public class CalendarMigrate
    {
        public int Id { get; set; }

        public int LeagueId { get; set; }

        public League League { get; set; }

        public string Season { get; set; }

        public int Week { get; set; }

        public string SeasonType { get; set; }

        public DateTimeOffset FirstGameStart { get; set; }

        public DateTimeOffset LastGameStart { get; set; }

        public DateTimeOffset CutoffDate { get; set; }
    }
}