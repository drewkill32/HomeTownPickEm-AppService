using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Commands
{
    public class AddUser
    {
        public class Command : IRequest<UserDto>
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public int? TeamId { get; set; }

            public ApplicationUser ToAppUser()
            {
                return new ApplicationUser
                {
                    Email = Email,
                    TeamId = TeamId,
                    Name = new UserName
                    {
                        First = FirstName,
                        Last = LastName
                    },
                    UserName = Email
                };
            }
        }

        public class CommandHandler : IRequestHandler<Command, UserDto>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;


            public CommandHandler(UserManager<ApplicationUser> userManager, ApplicationDbContext context,
                IMapper mapper)
            {
                _userManager = userManager;
                _context = context;
                _mapper = mapper;
            }

            public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.CreateUserAsync(request);
                return await _context.Users.Where(x => x.Id == user.Id)
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);
            }
        }
    }
}