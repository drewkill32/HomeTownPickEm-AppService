using System.Collections.Generic;
using System.Security.Claims;
using HomeTownPickEm.Models;

namespace HomeTownPickEm.Abstract.Interfaces
{
    public interface IJwtService
    {
        string CreateToken(ApplicationUser user, IList<Claim> claims);
        IEnumerable<Claim> GetClaims(string tokenString);
    }
}