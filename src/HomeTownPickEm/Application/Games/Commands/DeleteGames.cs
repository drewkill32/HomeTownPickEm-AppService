using HomeTownPickEm.Data;
using HomeTownPickEm.Services.Cfbd;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Games.Commands;

public class DeleteGames
{
    public class Command : IRequest
    {
        public string Year { get; set; } = DateTime.Now.Year.ToString();

    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {


            var games = await _context.Games.Where(x => x.Season == request.Year).ToArrayAsync(cancellationToken);
                
            _context.Games.RemoveRange(games);
            await _context.SaveChangesAsync(cancellationToken);
            
        }
    }
}