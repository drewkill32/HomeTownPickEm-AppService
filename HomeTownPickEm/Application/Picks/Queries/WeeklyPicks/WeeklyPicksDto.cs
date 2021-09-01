namespace HomeTownPickEm.Application.Picks.Queries.WeeklyPicks
{
    public class WeeklyPicksDto
    {
        public int Week { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string ProfileImg { get; set; }

        public int UnselectedPicks { get; set; }

        public int TotalPicks { get; set; }
    }
}