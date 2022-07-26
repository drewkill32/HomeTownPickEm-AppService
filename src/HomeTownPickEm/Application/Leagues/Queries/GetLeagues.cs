using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeTownPickEm.Application.Users;
using HomeTownPickEm.Data;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Queries;

public class GetLeagues
{
    public class Query : IRequest<IEnumerable<UserLeagueDto>>
    {
        public bool? Active { get; set; }
    }

    public class QueryHandler : IRequestHandler<Query, IEnumerable<UserLeagueDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public QueryHandler(ApplicationDbContext context, IUserAccessor userAccessor, IMapper mapper)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserLeagueDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = _userAccessor.GetCurrentUserId();
            var leagues = await _context.Season
                .Where(x => x.Members.Any(y => y.Id == userId))
                .Where(x => !request.Active.HasValue || x.Active == request.Active)
                .Select(x => x.League)
                .ProjectTo<UserLeagueDto>(_mapper.ConfigurationProvider)
                .ToArrayAsync(cancellationToken);
            return leagues;
        }
    }
}