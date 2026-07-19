using FluentValidation.TestHelper;
using Tests.Uinsure.Core;
using Uinsure.Core.Models;
using Uinsure.Core.Validators;

namespace Tests.Uinsure.Basic.Validation;

public class PolicyRenewalValidatorTests
{
    private readonly TimeProvider _timeProvider = Settings.TimeProvider;

    public PolicyRenewalValidatorTests()
    {
        
    }

    private PolicyRenewalValidator CreateValidator()
        => new(_timeProvider);

    [Fact]
    public void Policy_should_be_valid_for_renewal_when_not_yet_expired_and_within_30_days()
    {
        // arrange
        var policy = new HouseHoldPolicy { StartDate = DateOnly.Parse("2025-06-15") };
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(policy);

        // assert
        result.IsValid.Should().BeTrue();
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Policy_should_be_invalid_for_renewal_when_expired()
    {
        // arrange
        var policy = new HouseHoldPolicy { StartDate = DateOnly.Parse("2025-04-15") };
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(policy);

        // assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(p => p)
            .WithErrorMessage("Policy can not be renewed after the policy end date.");
    }

    [Fact]
    public void Policy_should_be_invalid_for_renewal_when_outside_of_renewal_period()
    {
        // arrange
        var policy = new HouseHoldPolicy { StartDate = DateOnly.Parse("2025-05-15") };
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(policy);

        // assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(p => p)
            .WithErrorMessage("Policy can only be renewed 30 days before end date.");
    }
}
