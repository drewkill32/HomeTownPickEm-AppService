using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Queries;

public class GetLeagueSettings
{
    public class Query : IRequest<LeagueSettingsDto>
    {
        public int LeagueId { get; set; }
    }

    public class QueryHandler : IRequestHandler<Query, LeagueSettingsDto>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public QueryHandler(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<LeagueSettingsDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var league = await _dbContext.League.Where(x => x.Id == request.LeagueId)
                .ProjectTo<LeagueSettingsDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            return league;
        }
    }
}