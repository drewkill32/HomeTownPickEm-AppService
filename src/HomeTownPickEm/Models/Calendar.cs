using System;
using System.Diagnostics;

namespace HomeTownPickEm.Models
{
    [DebuggerDisplay("{Season}-{Week}")]
    public class Calendar
    {

        public string Season { get; set; }

        public int Week { get; set; }

        public string SeasonType { get; set; }

        public DateTimeOffset FirstGameStart { get; set; }

        public DateTimeOffset LastGameStart { get; set; }
        
    }
}