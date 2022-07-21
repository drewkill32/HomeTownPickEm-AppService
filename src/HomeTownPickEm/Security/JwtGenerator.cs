using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HomeTownPickEm.Models;
using Microsoft.IdentityModel.Tokens;

namespace HomeTownPickEm.Security
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly SymmetricSecurityKey _key;
        public JwtGenerator(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(ApplicationUser user, params Claim[] additionalClaims)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.NameId, user.UserName),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.GivenName, user.Name.First),
                new(JwtRegisteredClaimNames.FamilyName, user.Name.Last),
                new("email_verified", user.EmailConfirmed.ToString())
            };

            claims.AddRange(additionalClaims);
            //generate signing credentials

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }

    public interface IJwtGenerator
    {
        string CreateToken(ApplicationUser user, params Claim[] additionalClaims);
    }
}