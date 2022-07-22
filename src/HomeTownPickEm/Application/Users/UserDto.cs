using AutoMapper;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Application.Teams;
using HomeTownPickEm.Models;

namespace HomeTownPickEm.Application.Users
{
    
    public static class UserExtensions
    {
        public static UserDto ToUserDto(this ApplicationUser user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.Name.First,
                LastName = user.Name.Last,
                Team = user.Team?.ToTeamDto() ?? new TeamDto(),
                //Leagues = user.Seasons.Select(x => x.GetLeagueName())
            };
        }

    
    }

    public class UserLeagueDto : IMapFrom<League>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public IEnumerable<string> Years { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<League, UserLeagueDto>()
                .ForMember(x => x.Years, exp => exp.MapFrom(y => y.Seasons.Select(x => x.Year)));
        }
    }
    
    public class UserDto : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }
        public string Email { get; set; }
        
        public TeamDto Team { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public IEnumerable<UserLeagueDto> Leagues { get; set; }


        public virtual void Mapping(Profile profile)
        {
            var m = profile.CreateMap<ApplicationUser, UserDto>();
            MapProperties(m);
        }

        protected void MapProperties<T>(IMappingExpression<ApplicationUser, T> exp) where T : UserDto
        {
            exp.ForMember(x => x.FirstName, exp => exp.MapFrom(s => s.Name.First))
                .ForMember(x => x.LastName, exp => exp.MapFrom(s => s.Name.Last))
                .ForMember(x => x.Leagues, exp => exp.MapFrom(s => s.Seasons.Select(x => x.League)));
        }
        
    }
}