using Microsoft.AspNetCore.Identity;

namespace HomeTownPickEm.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Seasons = new HashSet<Season>();
        }

        public ICollection<Season> Seasons { get; set; }
        public UserName Name { get; set; }

        public Team Team { get; set; }

        public int? TeamId { get; set; }

        public string ProfileImg { get; set; }
        
        public bool IsMigrated { get; set; }
    }
    public class PostgresApplicationUser : IdentityUser
    {
        public bool IsMigrated { get; set; }
    }
}
