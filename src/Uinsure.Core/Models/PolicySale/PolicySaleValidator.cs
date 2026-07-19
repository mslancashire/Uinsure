using FluentValidation;

namespace Uinsure.Core.Models.PolicySale;

internal class PolicySaleValidator : AbstractValidator<PolicySaleRequest>
{
    public PolicySaleValidator(TimeProvider timeProvider)
    {
        var now = timeProvider.GetUtcNow();
        var allowPoliciesTill = now.AddDays(60);

        RuleFor(r => r.StartDate)
            .NotNull()
            .WithMessage("Policy start date must be provided.");

        RuleFor(r => r.StartDate)
            .NotEmpty()
            .WithMessage("Policy start date must be provided.");

        RuleFor(r => r.StartDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(now.Date))
            .WithMessage("Policy must either start today or into the future.");

        RuleFor(r => r.StartDate)
            .LessThan(DateOnly.FromDateTime(allowPoliciesTill.Date))
            .WithMessage("Policy can only be sold up to 60 days in advance.");
    }
}
