using System.Text;
using System.Web;
using AutoMapper;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Commands
{
    public class Register
    {
        public class Command : IRequest<TokenDto>
        {
            public string Email { get; set; }

            public string Password { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }
        }

        public class Handler : IRequestHandler<Command, TokenDto>
        {
            private readonly ApplicationDbContext _context;
            private readonly ITokenService _tokenService;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly IEmailSender _emailSender;
            private readonly HttpContext _httpContext;

            public Handler(ApplicationDbContext context,
                UserManager<ApplicationUser> userManager,
                IMapper mapper,
                IEmailSender emailSender,
                IHttpContextAccessor accessor,
                ITokenService tokenService)
            {
                _userManager = userManager;
                _emailSender = emailSender;
                _tokenService = tokenService;
                _context = context;
                _httpContext = accessor.HttpContext;

            }

            public async Task<TokenDto> Handle(Command request, CancellationToken cancellationToken)
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

                return await _tokenService.GenerateNewTokens(user.Id, cancellationToken);
            }
        }
    }
}