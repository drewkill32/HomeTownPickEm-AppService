using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Application.Exceptions;
using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Picks.Queries
{
    public class GetPickQueryHandler : IRequestHandler<GetPickQuery, PickDto>
    {
        private readonly ApplicationDbContext _context;

        public GetPickQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PickDto> Handle(GetPickQuery request, CancellationToken cancellationToken)
        {
            var picks = await _context.Pick
                .Where(x => x.Id == request.Id)
                .Include(x => x.Game)
                .Include(x => x.Game.Away)
                .Include(x => x.Game.Home)
                .Include(x => x.User)
                .Include(x => x.SelectedTeam)
                .Include(x => x.League.Teams)
                .AsSplitQuery()
                .SingleOrDefaultAsync(cancellationToken);

            if (picks == null)
            {
                throw new NotFoundException("Pick", request.Id);
            }

            return picks.ToPickDto();
        }
    }

    public class GetPickQuery : IRequest<PickDto>
    {
        public int Id { get; set; }
    }
}