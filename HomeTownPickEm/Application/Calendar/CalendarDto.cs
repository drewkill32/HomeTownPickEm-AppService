using System;

namespace HomeTownPickEm.Application.Calendar
{
    public static class CalendarDtoExtensions
    {
        public static CalendarDto ToCalendarDto(this Models.Calendar calendar)
        {
            return new CalendarDto
            {
                Season = calendar.Season,
                SeasonType = calendar.SeasonType,
                Week = calendar.Week,
                FirstGameStart = calendar.FirstGameStart,
                LastGameStart = calendar.LastGameStart
            };
        }
    }

    public class CalendarDto
    {
        public string Season { get; set; }

        public int Week { get; set; }

        public string SeasonType { get; set; }

        public DateTimeOffset CutoffDate { get; set; }

        public DateTimeOffset FirstGameStart { get; set; }

        public DateTimeOffset LastGameStart { get; set; }
    }
}