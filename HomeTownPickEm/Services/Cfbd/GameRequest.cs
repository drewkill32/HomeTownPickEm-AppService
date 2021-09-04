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
            return $"year={Year}&seasonType={SeasonType}&week={Week}";
        }

        public override string ToString()
        {
            return ToQueryString();
        }
    }
}