using AutoMapper;
using HomeTownPickEm.Models;

namespace HomeTownPickEm.Application.Users;

public class TokenUserDto : UserDto
{
    public string Token { get; set; }

    public override void Mapping(Profile profile)
    {
        var exp = profile.CreateMap<ApplicationUser, TokenUserDto>();
        MapProperties(exp);
    }
}