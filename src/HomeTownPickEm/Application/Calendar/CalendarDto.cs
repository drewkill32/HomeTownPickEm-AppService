using AutoMapper;
using HomeTownPickEm.Abstract.Interfaces;

namespace HomeTownPickEm.Application.Calendar
{
    public class CalendarDto : IMapFrom<Models.Calendar>
    {

        public string Season { get; set; }

        public int Week { get; set; }

        public string SeasonType { get; set; }
        

        public DateTimeOffset FirstGameStart { get; set; }

        public DateTimeOffset LastGameStart { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Models.Calendar, CalendarDto>()
                .ReverseMap();
        }
    }
}