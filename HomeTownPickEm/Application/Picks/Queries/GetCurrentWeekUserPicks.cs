using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Data;
using HomeTownPickEm.Extensions;
using HomeTownPickEm.Models;
using HomeTownPickEm.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Application.Picks.Queries
{
    public class GetCurrentWeekUserPicks
    {
        public class Query : IRequest<IEnumerable<UserPicksDto>>
        {
            public string LeagueSlug { get; set; }
            public int Week { get; set; }

            public string Season { get; set; } = DateTime.Now.Year.ToString();
        }

        public class QueryHandler : IRequestHandler<Query, IEnumerable<UserPicksDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IEnumerable<UserPicksDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var date = DateTimeOffset.UtcNow;

                var cal = (await _context.Calendar
                        .Where(x => x.League.Slug == request.LeagueSlug
                                    && x.Week == request.Week
                                    && x.Season == request.Season)
                        .Select(x => new { x.CutoffDate, x.LeagueId })
                        .SingleOrDefaultAsync(cancellationToken))
                    .GuardAgainstNotFound();

                if (date < cal.CutoffDate)
                {
                    throw new ForbiddenAccessException(
                        $"You can not view picks before the cutoff date. {cal.CutoffDate:yyyy-M-d hh:mm}");
                }


                var userPicks = await _context.Pick
                    .Where(x => x.LeagueId == cal.LeagueId
                                && x.Game.Week == request.Week)
                    .ProjectTo<UserPicksDto>(_mapper.ConfigurationProvider,
                        new { cal.LeagueId })
                    .ToArrayAsync(cancellationToken);

                return userPicks;
            }
        }
    }

    public class UserPicksDto : IMapFrom<Pick>
    {
        public int Id { get; set; }

        public int Points { get; set; }

        public UserDto User { get; set; }
        public GameDto Game { get; set; }
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


        public class GameDto : IMapFrom<Game>
        {
            public int Id { get; set; }
            public int? HomePoints { get; set; }

            public TeamDto Home { get; set; }


            public TeamDto Away { get; set; }

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

        public class TeamDto : IMapFrom<Team>
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
                    .CreateMap<Team, TeamDto>()
                    .ForMember(dest => dest.Logo, opt => opt.MapFrom(src => src.Logos));
            }
        }

        public class UserDto : IMapFrom<ApplicationUser>
        {
            public string Id { get; set; }
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string FullName => $"{FirstName} {LastName}".Trim();


            public int TotalPoints { get; set; }

            public void Mapping(Profile profile)
            {
                var LeagueId = -1;
                profile
                    .CreateMap<ApplicationUser, UserDto>()
                    .ForMember(dest => dest.TotalPoints,
                        opt =>
                            opt.MapFrom(src =>
                                src.Leagues.Where(l => l.Id == LeagueId)
                                    .SelectMany(l => l.Picks)
                                    .Where(p => p.UserId == src.Id)
                                    .Sum(p => p.Points)));
            }

            public override string ToString()
            {
                return FullName;
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