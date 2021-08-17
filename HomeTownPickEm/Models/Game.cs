using System;
using System.Collections.Generic;

namespace HomeTownPickEm.Models
{
    public class Game
    {
        public int Id { get; set; }

        public string Season { get; set; }

        public int Week { get; set; }

        public string SeasonType { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public bool StartTimeTbd { get; set; }

        public int HomeId { get; set; }

        public int? HomePoints { get; set; }

        public int AwayId { get; set; }

        public Team Home { get; set; }

        public Team Away { get; set; }

        public int? AwayPoints { get; set; }
        public ICollection<Pick> Picks { get; set; }
    }
}