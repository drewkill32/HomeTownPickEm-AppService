using System.Linq;

namespace HomeTownPickEm.Models
{
    public record UserName
    {
        public string First { get; set; }

        public string Last { get; set; }

        public string Full => $"{First} {Last}";

        public string Initials => $"{First.FirstOrDefault()}{Last.FirstOrDefault()}";
    }
}