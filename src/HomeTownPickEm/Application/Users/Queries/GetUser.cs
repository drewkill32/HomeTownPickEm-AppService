using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeTownPickEm.Application.Exceptions;
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
            private readonly IMapper _mapper;

            public QueryHandler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .Where(x => x.Id == request.Id)
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(cancellationToken);

                if (user == null)
                {
                    throw new NotFoundException($"User {request.Id} not found");
                }
                return user;
            }
        }
    }
}