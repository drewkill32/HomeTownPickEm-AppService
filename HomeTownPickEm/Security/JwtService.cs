using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HomeTownPickEm.Security
{
    public class JwtService : IJwtService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtService(IConfiguration config, TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public IEnumerable<Claim> GetClaims(string tokenString)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claimsPrincipal = tokenHandler.ValidateToken(tokenString, _tokenValidationParameters, out var _);
            return claimsPrincipal.Claims;
        }


        public string CreateToken(ApplicationUser user, IList<Claim> claims)
        {
            if (claims.All(x => x.Type != JwtRegisteredClaimNames.NameId))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.UserName));
            }

            //generate signing credentials

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(100),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}