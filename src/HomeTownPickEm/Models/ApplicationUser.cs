using Microsoft.AspNetCore.Identity;

namespace HomeTownPickEm.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Seasons = new HashSet<Season>();
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public ICollection<Season> Seasons { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
        public UserName Name { get; set; }

        public Team Team { get; set; }

        public int? TeamId { get; set; }

        public string ProfileImg { get; set; }
    }
}