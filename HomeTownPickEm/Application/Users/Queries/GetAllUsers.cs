using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Users.Queries
{
    public class GetAllUsers
    {
        public class Query : IRequest<IEnumerable<UserDto>>
        {
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<UserDto>>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<UserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _context.Users.ToArrayAsync(cancellationToken);

                return users.Select(x => x.ToUserDto());
            }
        }
    }
}