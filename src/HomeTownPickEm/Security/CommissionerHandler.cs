using Microsoft.AspNetCore.Authorization;

namespace HomeTownPickEm.Security;

public class CommissionerHandler : AuthorizationHandler<CommissionerRequirement, int>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        CommissionerRequirement requirement, int resource)
    {
        var claims = context.User.FindAll(
                c => c.Type == Claims.Types.Commissioner)
            .ToArray();


        if (!claims.Any())
        {
            return Task.CompletedTask;
        }


        if (claims.Any(c => Convert.ToInt32(c.Value) == resource))
        {
            context.Succeed(requirement);
        }


        return Task.CompletedTask;
    }
}