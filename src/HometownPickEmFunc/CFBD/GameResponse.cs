using System;
using System.Diagnostics;

namespace HometownPickEmFunc.CFBD
{
    [DebuggerDisplay("[{Id}] {Week} {AwayId}-{HomeId}")]
    public class GameResponse
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
    }
}