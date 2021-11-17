using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Queries
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly ApplicationDbContext _context;

        public GetUserQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(x => x.Team)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return user.ToUserDto();
        }
    }

    public class GetUserQuery : IRequest<UserDto>
    {
        public string Id { get; set; }
    }
}