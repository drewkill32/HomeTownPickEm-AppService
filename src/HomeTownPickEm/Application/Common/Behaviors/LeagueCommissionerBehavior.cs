using HomeTownPickEm.Extensions;
using HomeTownPickEm.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace HomeTownPickEm.Application.Common.Behaviors;

public class LeagueCommissionerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IAuthorizationService _authService;
    private readonly IHttpContextAccessor _contextAccessor;


    public LeagueCommissionerBehavior(IAuthorizationService authService, IHttpContextAccessor contextAccessor)
    {
        _authService = authService;
        _contextAccessor = contextAccessor;
    }

    

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is ILeagueCommissionerRequest leagueCommissionerRequest)
        {
            var user = _contextAccessor.HttpContext.User;
            var result = await _authService.ValidateCommissioner(user, leagueCommissionerRequest.LeagueId);
            if (!result.Succeeded)
            {
                throw new ForbiddenAccessException("You are not authorized to access this resource");
            }
        }

        return await next();
    }
}