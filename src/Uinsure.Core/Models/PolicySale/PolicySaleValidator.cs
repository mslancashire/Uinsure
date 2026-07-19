using FluentValidation;

namespace Uinsure.Core.Models.PolicySale;

public class PolicySaleValidator : AbstractValidator<PolicySaleRequest>
{
    public PolicySaleValidator(TimeProvider timeProvider)
    {
        var now = DateOnly.FromDateTime(timeProvider.GetUtcNow().Date);
        var allowPoliciesTill = now.AddDays(60);

        RuleFor(r => r.StartDate)
            .NotNull()
            .WithMessage("Policy start date must be provided.");

        RuleFor(r => r.StartDate)
            .NotEmpty()
            .WithMessage("Policy start date must be provided.");

        RuleFor(r => r.StartDate)
            .GreaterThanOrEqualTo(now)
            .WithMessage("Policy must either start today or into the future.");

        RuleFor(r => r.StartDate)
            .LessThan(allowPoliciesTill)
            .WithMessage("Policy can only be sold up to 60 days in advance.");

        RuleFor(r => r.PolicyHolders)
            .NotEmpty()
            .WithMessage("Policy must have at least 1 Policy Holder.");

        RuleFor(r => r.PolicyHolders)
            .Must(items => items.Count() <= 3)
            .WithMessage("Policy can only have up to 3 Policy Holders.");

        RuleForEach(r => r.PolicyHolders)
            .SetValidator(policy => new PolicyHolderValidator(timeProvider, policy));
    }
}
