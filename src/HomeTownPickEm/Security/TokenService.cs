using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Users;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HomeTownPickEm.Security
{
    public class TokenService : ITokenService
    {
        private readonly IHostEnvironment _env;
        private readonly ApplicationDbContext _context;
        private readonly ISystemDate _date;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtOptions _opt;
        private readonly HttpContext _httpContext;

        public TokenService(IOptions<JwtOptions> options, IHostEnvironment env,
            ApplicationDbContext context, IHttpContextAccessor contextAccessor,
            ISystemDate date, UserManager<ApplicationUser> userManager)
        {
            _env = env;
            _context = context;
            _date = date;
            _userManager = userManager;
            _httpContext = contextAccessor.HttpContext;
            _opt = options.Value;
        }

        public (string JwtId, string Token) CreateToken(ApplicationUser user, params Claim[] additionalClaims)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.GivenName, user.Name.First),
                new(JwtRegisteredClaimNames.FamilyName, user.Name.Last),
                new("email_verified", user.EmailConfirmed.ToString())
            };

            claims.AddRange(additionalClaims);
            //generate signing credentials

            var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.Key)),
                SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _opt.Audience,
                Issuer = _opt.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = _env.IsDevelopment()
                    ? DateTime.UtcNow.AddSeconds(10)
                    : DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = creds,
                IssuedAt = _date.UtcNow.DateTime
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return (token.Id, tokenString);
        }


        public async Task<TokenDto> GenerateNewTokens(string id, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(x => x.Id == id)
                .Include(x => x.RefreshTokens)
                .AsTracking()
                .FirstOrDefaultAsync(cancellationToken);


            var claims = (await _userManager.GetClaimsAsync(user)).ToArray();

            var tokensToRemove = user.RefreshTokens.Where(x => x.ExpiryDate < _date.UtcNow).ToArray();
            foreach (var token in tokensToRemove)
            {
                user.RefreshTokens.Remove(token);
            }

            var expiryDate = DateTimeOffset.UtcNow.AddMonths(6);
            var (jwtId, accessToken) = CreateToken(user, claims);
            var dto = new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = CreateRefreshToken(),
                ExpiresIn = expiryDate.ToUnixTimeSeconds()
            };

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = dto.RefreshToken,
                ExpiryDate = expiryDate,
                UserId = user.Id,
                AddedDate = _date.UtcNow,
                JwtId = jwtId,
                IpAddress = _httpContext.Connection.RemoteIpAddress.ToString()
            });
            await _context.SaveChangesAsync(cancellationToken);
            return dto;
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidAudience = _opt.Audience,
                ValidIssuer = _opt.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.Key)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }

    public interface ITokenService
    {
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        Task<TokenDto> GenerateNewTokens(string id, CancellationToken cancellationToken);
    }
}