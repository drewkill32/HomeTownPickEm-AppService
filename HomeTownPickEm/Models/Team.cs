using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace HomeTownPickEm.Models
{
    [DebuggerDisplay("[{Id}] {School} {Mascot}")]
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string School { get; set; }
        public string Mascot { get; set; }
        public string Abbreviation { get; set; }
        public string Conference { get; set; }
        public string Division { get; set; }
        public string Color { get; set; }
        public string AltColor { get; set; }
        public string Logos { get; set; }
    }
    // {
    // "id": 2000,
    // "school": "Abilene Christian",
    // "mascot": "Wildcats",
    // "abbreviation": "ACU",
    // "alt_name1": null,
    // "alt_name2": "ACU",
    // "alt_name3": "Abil Christian",
    // "conference": null,
    // "division": null,
    // "color": "#4e2683",
    // "alt_color": "#ebebeb",
    // "logos": [
    // "http://a.espncdn.com/i/teamlogos/ncaa/500/2000.png",
    // "http://a.espncdn.com/i/teamlogos/ncaa/500-dark/2000.png"
    // ],
    // "location": {
    // "venue_id": null,
    // "name": null,
    // "city": null,
    // "state": null,
    // "zip": null,
    // "country_code": null,
    // "timezone": null,
    // "latitude": null,
    // "longitude": null,
    // "elevation": null,
    // "capacity": null,
    // "year_constructed": null,
    // "grass": null,
    // "dome": null
    // }
}