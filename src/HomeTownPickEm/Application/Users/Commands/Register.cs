using System.Text;
using System.Web;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using HomeTownPickEm.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Commands
{
    public class Register
    {
        public class Command : IRequest<UserDto>
        {
            public string Email { get; set; }

            public string Password { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }
        }

        public class Handler : IRequestHandler<Command, UserDto>
        {
            private readonly ApplicationDbContext _context;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IMapper _mapper;
            private readonly IEmailSender _emailSender;
            private readonly HttpContext? _httpContext;

            public Handler(ApplicationDbContext context,
                ILeagueServiceFactory leagueServiceFactory,
                UserManager<ApplicationUser> userManager,
                IMapper mapper,
                IEmailSender emailSender,
                IHttpContextAccessor accessor,
                IJwtGenerator jwtGenerator)
            {
                _userManager = userManager;
                _mapper = mapper;
                _emailSender = emailSender;
                _jwtGenerator = jwtGenerator;
                _context = context;
                _httpContext = accessor.HttpContext;

            }

            public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Users.AnyAsync(x => x.UserName == request.Email, cancellationToken))
                {
                    throw new BadRequestException("Username already exists");
                }
                

                var user = new ApplicationUser
                {
                    Email = request.Email,
                    UserName = request.Email,
                    Name = new UserName
                    {
                        First = request.FirstName,
                        Last = request.LastName
                    }
                };
                

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    throw new BadRequestException(string.Join(". ", result.Errors.Select(x => x.Description)));
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var webCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var url =
                    $"{_httpContext.Request.Scheme}://{_httpContext.Request.Host}/confirm-email?code={webCode}&email={HttpUtility.UrlEncode(user.Email)}";

                var htmlMessage =
                    $"Click <a href=\"{url}\">here</a> to confirm your email. If you did not generate this request ignore this email.";

                await _emailSender.SendEmailAsync(user.Email, "St. Pete Pick'em Reset Password", htmlMessage);
                var fullUser = await _context.Users
                    .ProjectTo<TokenUserDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken);
                fullUser.Token = _jwtGenerator.CreateToken(user);
                return fullUser;
                

            }
        }
    }
}