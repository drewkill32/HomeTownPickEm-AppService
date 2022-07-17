using AutoMapper;
using AutoMapper.QueryableExtensions;
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
            private readonly IMapper _mapper;

            public QueryHandler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IEnumerable<UserDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _context.Users
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    .ToArrayAsync(cancellationToken);

                return users;
            }
        }
    }
}