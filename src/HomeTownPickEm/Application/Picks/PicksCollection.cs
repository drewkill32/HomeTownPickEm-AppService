using System;
using System.Collections.Generic;
using System.Linq;
using HomeTownPickEm.Models;

namespace HomeTownPickEm.Application.Picks
{
    public static class PickDtoExtensions
    {
        public static PicksCollection ToPicksDto(this Pick[] picks, DateTimeOffset cutoffDate)
        {
            if (picks.Length == 0)
            {
                return new PicksCollection();
            }

            return new PicksCollection
            {
                CutoffDate = cutoffDate,
                Picks = picks.Select(x => x.ToPickDto())
            };
        }
    }

    public class PicksCollection
    {
        public PicksCollection()
        {
            Picks = new HashSet<PickDto>();
        }

        public int Count => Picks.Count();
        public DateTimeOffset CutoffDate { get; set; }

        public IEnumerable<PickDto> Picks { get; set; }
    }
}