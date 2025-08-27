using System;

namespace HomeTownPickEm.Services.Cfbd
{
    public class GameRequest
    {
        public string Year { get; set; } = DateTime.Now.Year.ToString();

        public int? Week { get; set; }

        public string SeasonType { get; set; } = "regular";

        public string ToQueryString()
        {
            var str =  $"year={Year}&seasonType={SeasonType}";
            if (Week.HasValue)
            {
                str += $"&week={Week.Value}";
            }
            return str;
        }

        public override string ToString()
        {
            return ToQueryString();
        }
    }
}