using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Queries.Profile
{
    public class ProfileQuery : IRequest<ProfileVm>
    {
    }

    public class ProfileQueryHandler : IRequestHandler<ProfileQuery, ProfileVm>
    {
        private readonly ApplicationDbContext _context;

        private readonly HttpContext _httpContext;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileQueryHandler(IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<ProfileVm> Handle(ProfileQuery request, CancellationToken cancellationToken)
        {
            var username = _httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                throw new UnauthorizedAccessException(
                    "There was no 'ClaimTypes.NameIdentifier' claim found for the current user");
            }

            var profile = await GetProfile(username, cancellationToken);

            return profile;
        }

        private async Task<ProfileVm> GetProfile(string username,
            CancellationToken cancellationToken)
        {
            var profile = await _userManager.Users.Where(x => x.UserName == username)
                .ProjectTo<ProfileVm>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            profile.Roles = await _context.UserClaims
                .Where(x => x.UserId == profile.Id && x.ClaimType == ClaimTypes.Role)
                .Select(x => x.ClaimValue)
                .ToArrayAsync(cancellationToken);

            return profile;
        }
    }
}