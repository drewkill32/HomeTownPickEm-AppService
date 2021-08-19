using System;
using System.Collections.Generic;
using System.Diagnostics;
using HomeTownPickEm.Application.Teams;

namespace HomeTownPickEm.Models
{
    public static class TeamExtensions
    {
        public static TeamDto ToTeamDto(this Team team)
        {
            return new()
            {
                Abbreviation = team.Abbreviation,
                Color = team.Color,
                Conference = team.Conference,
                Division = team.Division,
                Mascot = team.Mascot,
                Id = team.Id,
                Logos = team.Logos.Split(';', StringSplitOptions.RemoveEmptyEntries),
                School = team.School,
                AltColor = team.AltColor
            };
        }
    }

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
        public ICollection<Pick> Picks { get; set; }
    }
}