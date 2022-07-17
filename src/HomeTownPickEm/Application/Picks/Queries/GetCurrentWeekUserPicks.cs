using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Data;
using HomeTownPickEm.Data.Extensions;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Picks.Queries
{
    public class GetCurrentWeekUserPicks
    {
        public class Query : IRequest<IEnumerable<UserPickCollection>>
        {
            public string LeagueSlug { get; set; }
            public int Week { get; set; }

            public string Season { get; set; } = DateTime.Now.Year.ToString();
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<UserPickCollection>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IEnumerable<UserPickCollection>> Handle(Query request,
                CancellationToken cancellationToken)
            {
                var date = DateTimeOffset.UtcNow;

                var seasonId =
                    await _context.Season.GetLeagueSeasonId(request.Season, request.LeagueSlug, cancellationToken);
                var cutoffDate = (await _context.Calendar
                        .Where(x => x.Week == request.Week
                                    && x.Season == request.Season)
                        .Select(x => x.FirstGameStart)
                        .SingleOrDefaultAsync(cancellationToken))
                    .GuardAgainstNotFound();

                if (date < cutoffDate)
                {
                    throw new ForbiddenAccessException(
                        $"You can not view picks before the cutoff date. {cutoffDate:yyyy-M-d hh:mm}");
                }

                var users = await _context.Users
                    .Where(x => x.Seasons.Any(l => l.Id == seasonId))
                    .ProjectTo<UserPicksDto.UserProjection>(_mapper.ConfigurationProvider)
                    .ToArrayAsync(cancellationToken);


                var userPicks = (await _context.Pick
                        .Where(x => x.SeasonId == seasonId
                                    && x.Game.Week == request.Week)
                        .ProjectTo<UserPicksDto>(_mapper.ConfigurationProvider,
                            new { seasonId })
                        .ToArrayAsync(cancellationToken))
                    .OrderBy(x => x.Game.StartDate)
                    .ThenBy(x => x.Game.Home.Name)
                    .ToArray();

                var collection = users.Select(x => new UserPickCollection
                    {
                        User = x,
                        Picks = userPicks.Where(p => p.UserId == x.Id).ToArray()
                    })
                    .OrderByDescending(x => x.User.TotalPoints)
                    .ThenBy(x => x.User.FullName)
                    .ToArray();

                return collection;
            }
        }
    }

    public class UserPickCollection
    {
        public UserPickCollection()
        {
            Picks = new HashSet<UserPicksDto>();
        }

        public UserPicksDto.UserProjection User { get; set; }

        public ICollection<UserPicksDto> Picks { get; set; }
    }

    public class UserPicksDto : IMapFrom<Pick>
    {
        public int Id { get; set; }

        public int Points { get; set; }
        public string UserId { get; set; }
        public GameProjection Game { get; set; }
        public string SelectedTeam { get; set; }
        public int SelectedTeamId { get; set; }

        public string Status
        {
            get
            {
                if (!Game.IsFinal)
                {
                    return PickStatus.Pending;
                }

                return SelectedTeamId == Game.WinnerId ? PickStatus.Win : PickStatus.Loss;
            }
        }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Pick, UserPicksDto>()
                .ForMember(dest => dest.SelectedTeam, opt =>
                    opt.MapFrom(src => src.SelectedTeamId == src.Game.HomeId ? "Home" : "Away"));
        }


        public class GameProjection : IMapFrom<Game>
        {
            public int Id { get; set; }
            public int? HomePoints { get; set; }

            public TeamProjection Home { get; set; }


            public TeamProjection Away { get; set; }

            public DateTimeOffset StartDate { get; set; }

            public int? AwayPoints { get; set; }

            public bool IsFinal => AwayPoints.HasValue && HomePoints.HasValue;


            public string Winner
            {
                get
                {
                    if (!IsFinal)
                    {
                        return "Pending";
                    }

                    return HomePoints > AwayPoints ? nameof(Home) : nameof(Away);
                }
            }

            public int WinnerId
            {
                get
                {
                    if (!IsFinal)
                    {
                        return 0;
                    }

                    return HomePoints > AwayPoints ? Home.Id : Away.Id;
                }
            }
        }

        public class TeamProjection : IMapFrom<Team>
        {
            private string _logo;
            public int Id { get; set; }

            public string School { get; set; }

            public string Mascot { get; set; }

            public string Name => $"{School} {Mascot}";

            public string Logo
            {
                get => _logo;
                set => _logo = LogoHelper.GetSingleLogo(value);
            }

            public void Mapping(Profile profile)
            {
                profile
                    .CreateMap<Team, TeamProjection>()
                    .ForMember(dest => dest.Logo, opt =>
                        opt.MapFrom(src => src.Logos));
            }
        }

        public class UserProjection : IMapFrom<ApplicationUser>
        {
            public string Id { get; set; }

            public string ProfileImg { get; set; }
            
            public string FullName { get; set; }


            public int TotalPoints { get; set; }

            public void Mapping(Profile profile)
            {
                profile
                    .CreateMap<ApplicationUser, UserProjection>()
                    .ForMember(dest=> dest.FullName,
                        cfg=> 
                            cfg.MapFrom(src=> src.Name.Full))
                    .ForMember(dest => dest.TotalPoints,
                        opt =>
                            opt.MapFrom(src =>
                                src.Seasons
                                    .SelectMany(l => l.Picks)
                                    .Where(p => p.UserId == src.Id)
                                    .Sum(p => p.Points)));
            }
            
        }
    }

    public class PickStatus
    {
        public const string Pending = "Pending";
        public const string Win = "Win";
        public const string Loss = "Loss";
    }
}