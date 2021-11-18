using System;
using System.Collections.Generic;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Models;

namespace HomeTownPickEm.Application.Users.Queries.Profile
{
    public class ProfileVm : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public UserName Name { get; set; }

        public string ProfileImg { get; set; }

        public TeamVm Team { get; set; }

        public ICollection<LeagueVm> Leagues { get; set; }

        public ICollection<string> Roles { get; set; } = Array.Empty<string>();
    }

    public class TeamVm : IMapFrom<Team>
    {
        public int Id { get; set; }

        public string School { get; set; }

        public string Mascot { get; set; }

        public string Logos { get; set; }
    }

    public class LeagueVm : IMapFrom<League>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string Season { get; set; }
    }
}