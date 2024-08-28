using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Security;
using HomeTownPickEm.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Commands;

public class ResendInvite
{
    public class Command : IRequest
    {
        public string Email { get; set; }
    }


    public class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailTemplateFactory _templateFactory;
        private readonly IEmailSender _sender;

        public Handler(ApplicationDbContext context, IUserAccessor userAccessor,
            UserManager<ApplicationUser> userManager, EmailTemplateFactory templateFactory, IEmailSender sender)
        {
            _context = context;
            _userAccessor = userAccessor;
            _userManager = userManager;
            _templateFactory = templateFactory;
            _sender = sender;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var principal = _userAccessor.GetClaimsPrincipal();
            if (!principal.Claims.Any(c => c is { Type: Claims.Types.Admin, Value: Claims.Values.True }))
            {
                throw new ForbiddenAccessException("User must be an admin");
            }

            var user = (await _userManager.FindByEmailAsync(request.Email))
                .GuardAgainstNotFound("No User Found");

            var pendingInvites = await _context.PendingInvites
                .Where(x => x.UserId == user.Id)
                .ToArrayAsync(cancellationToken);


            if (!pendingInvites.Any())
            {
                throw new NotFoundException(
                    $"There are no invites to be sent for {user.Email}");
            }

            var emailTemplate = await _templateFactory.CreateEmailTemplate(EmailType.NewUser, user);

            await _sender.SendEmailAsync(user.Email, emailTemplate.Subject, emailTemplate.HtmlMessage);
        }
    }
}