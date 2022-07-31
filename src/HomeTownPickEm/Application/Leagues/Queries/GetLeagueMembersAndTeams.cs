using HomeTownPickEm.Data;
using HomeTownPickEm.Security;
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

        public record LeagueMember(string FirstName, string LastName, string Initials, string FullName,
            string ProfileImg, string Id, string Email, bool IsCommissioner, string Color)
        {
            public bool IsCommissioner { get; set; } = IsCommissioner;
        }

        public async Task<object> Handle(Query request, CancellationToken cancellationToken)
        {
            var league = await _context.Season.Where(y => y.Year == request.Season && y.LeagueId == request.Id)
                .Select(x => new
                {
                    Members = x.Members.Select(m => new LeagueMember(m.Name.First,
                        m.Name.Last, 
                        m.Name.Initials,
                        m.Name.Full, 
                        m.ProfileImg,
                        m.Id, 
                        m.Email,
                        false,
                        m.Team != null ? m.Team.AltColor : "")),
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

            await AssignCommissioner(league.Members, request.Id.ToString(), cancellationToken);


            return new
            {
                Members = league.Members.OrderBy(m => m.FullName).ToArray(),
                Teams = league.Teams.OrderBy(t => t.Name).ToArray()
            };
        }

        private async Task AssignCommissioner(IEnumerable<LeagueMember> members,
            string leagueId,
            CancellationToken cancellationToken)
        {
            var userIds = members.Select(x => x.Id).ToArray();

            var claims = await _context.UserClaims.Where(x => userIds.Contains(x.UserId))
                .Where(x => x.ClaimType == Claims.Types.Commissioner && x.ClaimValue == leagueId)
                .ToArrayAsync(cancellationToken);

            var commissioners = claims.Select(x => x.UserId).ToArray();
            var commissionersMembers = members.Where(x => commissioners.Contains(x.Id)).ToArray();
            foreach (var commish in commissionersMembers)
            {
                commish.IsCommissioner = true;
            }
        }
    }
}