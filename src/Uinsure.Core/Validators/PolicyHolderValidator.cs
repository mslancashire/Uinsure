using FluentValidation;
using Uinsure.Core.Models;

namespace Uinsure.Core.Validators;

internal class PolicyHolderValidator : AbstractValidator<PolicyHolder>
{
    public PolicyHolderValidator(TimeProvider timeProvider, PolicySaleRequest policy)
    {
        RuleFor(r => r.FirstName)
            .NotEmpty()
            .WithMessage("Policy Holder's first name must be provided.");

        RuleFor(r => r.LastName)
            .NotEmpty()
            .WithMessage("Policy Holder's last name must be provided.");

        RuleFor(r => r.DateOfBirth)
            .NotEmpty()
            .WithMessage("Policy Holder's data of birth must be provided.");

        RuleFor(r => r.DateOfBirth)
            .Must(dob =>
            {
                if (policy is null || policy.StartDate == default)
                {
                    return false;
                }

                int age = policy.StartDate.Year - dob.Year;
                if (policy.StartDate < dob.AddYears(age))
                {
                    age--;
                }

                if (age >= 16)
                {
                    return true;
                }

                return false;
            })
            .WithMessage("Policy Holder must be over 16 years of age.");
    }
}
