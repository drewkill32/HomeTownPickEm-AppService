using System.Collections.Generic;

namespace HomeTownPickEm.Application.Teams.Commands
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string School { get; set; }
        public string Mascot { get; set; }
        public string Abbreviation { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
        public string Color { get; set; }
        public string AltColor { get; set; }
        public IEnumerable<string> Logos { get; set; }

        public string Name => $"{School} {Mascot}";
    }
}