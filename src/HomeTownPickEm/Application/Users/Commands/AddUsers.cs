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
    public class AddUsers
    {
        public class Command : List<AddUser.Command>, IRequest<IEnumerable<UserDto>>
        {
        }

        public class CommandHandler : IRequestHandler<Command, IEnumerable<UserDto>>
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

            public async Task<IEnumerable<UserDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var users = await _userManager.CreateUsersAsync(request);
                var userIds = users.Select(x => x.Id).ToArray();
                return await _context.Users.Where(x => userIds.Contains(x.Id))
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    .ToArrayAsync(cancellationToken);
            }
        }
    }
}