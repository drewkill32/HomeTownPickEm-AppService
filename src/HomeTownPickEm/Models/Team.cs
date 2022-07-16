using System.Collections.Generic;
using System.Diagnostics;

namespace HomeTownPickEm.Models
{
    

    [DebuggerDisplay("[{Id}] {School} {Mascot}")]
    public class Team
    {
        public Team()
        {
            Leagues = new HashSet<League>();
        }

        public int Id { get; set; }

        public string School { get; set; }
        public string Mascot { get; set; }
        public string Abbreviation { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
        public string Color { get; set; }
        public string AltColor { get; set; }
        public string Logos { get; set; }
        public ICollection<League> Leagues { get; set; }
    }
}