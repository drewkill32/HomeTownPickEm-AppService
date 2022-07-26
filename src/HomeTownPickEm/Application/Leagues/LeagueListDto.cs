using AutoMapper;
using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Models;

namespace HomeTownPickEm.Application.Leagues;

public class LeagueSettingsDto : IMapFrom<League>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }

    public string ImageUrl { get; set; }
    public IEnumerable<LeagueSettingsYearDto> Years { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<League, LeagueSettingsDto>()
            .ForMember(x => x.Years,
                exp =>
                    exp.MapFrom(y => y.Seasons.Select(x => new LeagueSettingsYearDto
                    {
                        Year = x.Year,
                        MemberCount = x.Members.Count(),
                        TeamCount = x.Teams.Count()
                    })));
    }
}

public class LeagueSettingsYearDto
{
    public string Year { get; set; }
    public int TeamCount { get; set; }
    public int MemberCount { get; set; }
}