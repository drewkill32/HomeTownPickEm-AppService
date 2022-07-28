using AutoMapper;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Commands;

public class InviteUser
{
    public class Command : ILeagueCommissionerRequest
    {
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public int LeagueId { get; set; }

        public string Season { get; set; }
        public int? TeamId { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailSender _sender;
        private readonly ILogger<InviteUser> _logger;

        public Handler(UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IMapper mapper, IEmailSender sender, ILogger<InviteUser> logger)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _sender = sender;
            _logger = logger;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
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
            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception(
                    $"Problem inviting user. {string.Join(", ", result.Errors.Select(x => x.Description))}");
            }


            try
            {
                _context.PendingInvites.Add(new PendingInvite
                {
                    LeagueId = request.LeagueId,
                    Season = request.Season,
                    UserId = user.Id,
                    TeamId = request.TeamId
                });
                await _context.SaveChangesAsync(cancellationToken);
                await _sender.SendEmailAsync(EmailType.NewUser, user);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Error creating user Invite. Attempting to rollback {ErrorMessage}", e.Message);
                var pendingInvite = await _context.PendingInvites
                    .Where(x => x.UserId == user.Id)
                    .ToArrayAsync(cancellationToken);
                _context.PendingInvites.RemoveRange(pendingInvite);
                await _context.SaveChangesAsync(cancellationToken);
                await _userManager.DeleteAsync(user);
            }

            return Unit.Value;
        }
    }
}