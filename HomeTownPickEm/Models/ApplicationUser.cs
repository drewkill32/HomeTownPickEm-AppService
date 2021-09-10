using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HomeTownPickEm.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Leagues = new HashSet<League>();
        }

        public ICollection<League> Leagues { get; set; }


        public UserName Name { get; set; }

        public Team Team { get; set; }

        public int? TeamId { get; set; }

        public string ProfileImg { get; set; }
    }
}