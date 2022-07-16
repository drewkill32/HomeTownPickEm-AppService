using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Queries
{
    public class GetUser
    {
        public class Query : IRequest<UserDto>
        {
            public string Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, UserDto>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .Include(x => x.Team)
                    .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                return user.ToUserDto();
            }
        }
    }
}