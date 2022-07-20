using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Commands;

public class VerifyPasswordReset
{
    public class Command : IRequest<UserDto>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command, UserDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<VerifyPasswordReset> _logger;

        public CommandHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, IMapper mapper,
            ILogger<VerifyPasswordReset> logger)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogError("User with email {Email} not found.", request.Email);
                throw new NotFoundException($"Unable to load user with email '{request.Email}'");
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            var result = await _userManager.ResetPasswordAsync(user, code, request.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("Reset password for {Email}", request.Email);
                return await _dbContext.Users
                    .Where(x => x.Id == user.Id)
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            _logger.LogError("Unable to verify user password reset token for user '{Email}'. {Errors}", request.Email,
                string.Join(", ", result.Errors.Select(x => x.Description)));
            throw new BadRequestException("Invalid token");
        }
    }
}