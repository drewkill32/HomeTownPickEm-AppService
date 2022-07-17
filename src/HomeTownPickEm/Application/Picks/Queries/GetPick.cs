using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Picks.Queries
{
    public class GetPick
    {
        public class Query : IRequest<PickDto>
        {
            public int Id { get; set; }
        }

        public class QueryHandler : IRequestHandler<Query, PickDto>
        {
            private readonly ApplicationDbContext _context;

            public QueryHandler(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<PickDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var picks = await _context.Pick
                    .Where(x => x.Id == request.Id)
                    .Include(x => x.Game)
                    .Include(x => x.Game.Away)
                    .Include(x => x.Game.Home)
                    .Include(x => x.User)
                    .Include(x => x.SelectedTeam)
                    .Include(x => x.Season.Teams)
                    .AsSplitQuery()
                    .SingleOrDefaultAsync(cancellationToken);

                if (picks == null)
                {
                    throw new NotFoundException("Pick", request.Id);
                }

                return picks.ToPickDto();
            }
        }
    }
}