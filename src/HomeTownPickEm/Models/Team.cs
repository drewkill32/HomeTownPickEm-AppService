using System.Diagnostics;
using HomeTownPickEm.Application.Common;

namespace HomeTownPickEm.Models
{
    

    [DebuggerDisplay("[{Id}] {School} {Mascot}")]
    public class Team : IHasId
    {
        public Team()
        {
            Seasons = new HashSet<Season>();
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
        public ICollection<Season> Seasons { get; set; }

        public override string ToString()
        {
            return $"{School} {Mascot}";
        }
    }
}