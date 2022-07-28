using MediatR;

namespace HomeTownPickEm.Application.Common;

public interface ILeagueCommissionerRequest : IRequest
{
    public int LeagueId { get; set; }
}