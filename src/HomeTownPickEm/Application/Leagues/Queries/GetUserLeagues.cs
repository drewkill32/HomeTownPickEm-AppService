using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeTownPickEm.Application.Users;
using HomeTownPickEm.Data;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Queries;

public class GetUserLeagues
{
    public class Query : IRequest<IEnumerable<UserLeagueDto>>
    {
    }

    public class Handler : IRequestHandler<Query, IEnumerable<UserLeagueDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserAccessor _accessor;
        private readonly IMapper _mapper;

        public Handler(ApplicationDbContext context, IUserAccessor accessor, IMapper mapper)
        {
            _context = context;
            _accessor = accessor;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserLeagueDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = _accessor.GetCurrentUserId();
            var leagues = await _context.League.Where(x => x.Seasons.Any(s => s.Members.Any(m => m.Id == userId)))
                .ProjectTo<UserLeagueDto>(_mapper.ConfigurationProvider)
                .ToArrayAsync(cancellationToken);

            return leagues;
        }
    }
}