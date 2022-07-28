using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HomeTownPickEm.Security;

public static class AuthorizationServiceExtensions
{
    public static async Task<AuthorizationResult> ValidateCommissioner(this IAuthorizationService authService,
        ClaimsPrincipal user, int leagueId)
    {
        return
            await authService.AuthorizeAsync(user, leagueId, Policies.LeagueCommissioner);
    }
}