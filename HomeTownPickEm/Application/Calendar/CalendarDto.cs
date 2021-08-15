using System;

namespace HomeTownPickEm.Application.Calendar
{
    public class CalendarDto
    {
        public string Season { get; set; }

        public int Week { get; set; }

        public string SeasonType { get; set; }

        public DateTimeOffset FirstGameStart { get; set; }

        public DateTimeOffset LastGameStart { get; set; }
    }
}