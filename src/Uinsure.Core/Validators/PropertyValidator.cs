using FluentValidation;
using Uinsure.Core.Models;

namespace Uinsure.Core.Validators;

public class PropertyValidator : AbstractValidator<Property>
{
    public PropertyValidator()
    {
        RuleFor(r => r.AddressLine1)
            .NotEmpty()
            .WithMessage("Must provide address line 1.");

        RuleFor(r => r.Postcode)
            .NotEmpty()
            .WithMessage("Must provide postcode.");

        RuleFor(r => r.Postcode)
            .MaximumLength(8)
            .WithMessage("Postcode must not be longer than 8 characters.");
    }
}
