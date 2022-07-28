using HomeTownPickEm.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Leagues.Queries;

public class GetLeagueMembersAndTeams
{
    public class Query : IRequest<object>
    {
        public int Id { get; set; }
        public string Season { get; set; }
    }

    public class Handler : IRequestHandler<Query, object>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> Handle(Query request, CancellationToken cancellationToken)
        {
            var league = await _context.Season.Where(y => y.Year == request.Season && y.LeagueId == request.Id)
                .Select(x => new
                {
                    Members = x.Members.Select(m => new
                    {
                        FirstName = m.Name.First,
                        LastName = m.Name.Last,
                        m.Name.Initials,
                        FullName = m.Name.Full,
                        m.ProfileImg,
                        m.Id,
                        m.Email,
                        Color = m.Team != null ? m.Team.AltColor : ""
                    }),
                    Teams = x.Teams.Select(t => new
                    {
                        t.Id,
                        t.Color,
                        t.AltColor,
                        Logo = t.Logos,
                        t.School,
                        t.Mascot,
                        Name = t.School + " " + t.Mascot
                    })
                })
                .FirstOrDefaultAsync(cancellationToken);


            return new
            {
                Members = league.Members.OrderBy(m => m.FullName).ToArray(),
                Teams = league.Teams.OrderBy(t => t.Name).ToArray()
            };
        }
    }
}