using FluentValidation;
using HomeTownPickEm.Application.Picks.Queries;

namespace HomeTownPickEm.Application.Picks.Validators;

public class GetUserPicksValidator : AbstractValidator<GetUserPicks.Query>
{
    public GetUserPicksValidator()
    {
        RuleFor(x => x.Season).NotEmpty();
        RuleFor(x => x.LeagueId).NotEmpty();
        RuleFor(x => x.Week).NotEmpty();
    }
}