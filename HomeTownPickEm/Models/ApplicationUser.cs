using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HomeTownPickEm.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<League> Leagues { get; set; }
    }
}