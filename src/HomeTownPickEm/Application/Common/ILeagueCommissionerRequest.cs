using MediatR;

namespace HomeTownPickEm.Application.Common;

public interface ILeagueCommissionerRequest : IBaseLeagueCommissionerRequest, IRequest
{
}

public interface IBaseLeagueCommissionerRequest
{
    public int LeagueId { get; set; }
}

public interface ILeagueCommissionerRequest<out T> : IBaseLeagueCommissionerRequest, IRequest<T>
{
}