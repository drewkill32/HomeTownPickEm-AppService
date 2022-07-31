using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using HomeTownPickEm.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
            private readonly UserManager<ApplicationUser> _userManager;

            public QueryHandler(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
            {
                _context = context;
                _mapper = mapper;
                _userManager = userManager;
            }

            public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .Where(x => x.Id == request.Id)
                    .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(cancellationToken);


                user.Claims = (await _userManager.GetClaimsAsync(new ApplicationUser
                    {
                        Id = user.Id
                    }))
                    .ToDictionary(x => x.Type.ToLower(), x => x.Value.ToLower());


                if (user == null)
                {
                    throw new NotFoundException($"User {request.Id} not found");
                }
                return user;
            }
        }
    }
}