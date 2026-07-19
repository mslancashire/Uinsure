using FluentValidation;
using Uinsure.Core.Models;

namespace Uinsure.Core.Validators;

public class PolicyRenewalValidator : AbstractValidator<HouseHoldPolicy>
{
    private readonly TimeProvider _timeProvider;

    public PolicyRenewalValidator(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        var now = DateOnly.FromDateTime(_timeProvider.GetUtcNow().Date);

        RuleFor(p => p)
            .Must(p => p.Expired(now) == false)
            .WithMessage($"Policy can not be renewed after the policy end date.");

        RuleFor(p => p)
            .Must(p => p.WithinRenewalPeriod(now))
            .WithMessage("Policy can only be renewed 30 days before end date.");
    }
}
