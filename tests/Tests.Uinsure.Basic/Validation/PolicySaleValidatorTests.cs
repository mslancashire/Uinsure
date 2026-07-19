using FluentValidation.TestHelper;
using Tests.Uinsure.Core;
using Tests.Uinsure.Core.Fakes;
using Uinsure.Core.Models.PolicySale;

namespace Tests.Uinsure.Basic.Validation;

public class PolicySaleValidatorTests
{
    private readonly TimeProvider _timeProvider = Settings.TimeProvider;
    private readonly FakePolicySaleRequests _faker;

    public PolicySaleValidatorTests()
    {
        _faker = new FakePolicySaleRequests(_timeProvider);
    }

    private PolicySaleValidator CreateValidator()
        => new(_timeProvider);

    [Fact]
    public void SalesRequest_should_be_valid()
    {
        // arrange
        var validSalesRequest = _faker.Valid();
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(validSalesRequest);

        // assert
        result.IsValid.Should().BeTrue();
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void SalesRequest_should_be_valid_when_start_date_is_today()
    {
        // arrange
        var validSalesRequest = _faker.ForToday();
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(validSalesRequest);

        // assert
        result.IsValid.Should().BeTrue();
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void SalesRequest_should_be_invalid_when_start_date_is_not_provided()
    {
        var validSalesRequest = _faker.MissingStartDate();
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(validSalesRequest);

        // assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(r => r.StartDate)
            .WithErrorMessage("Policy start date must be provided.");

    }

    [Fact]
    public void SalesRequest_should_be_invalid_when_start_date_is_in_past()
    {
        // arrange
        var validSalesRequest = _faker.InPast();
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(validSalesRequest);

        // assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(r => r.StartDate)
            .WithErrorMessage("Policy must either start today or into the future.");
    }

    [Fact]
    public void SalesRequest_should_be_invalid_when_start_date_is_over_60_days_into_future()
    {
        // arrange
        var validSalesRequest = _faker.Over60Days();
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(validSalesRequest);

        // assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(r => r.StartDate)
            .WithErrorMessage("Policy can only be sold up to 60 days in advance.");
    }

    [Fact]
    public void SalesRequest_should_be_invalid_when_start_date_exactly_60_days_into_future()
    {
        // arrange
        var validSalesRequest = _faker.Exactly60Days();
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(validSalesRequest);

        // assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(r => r.StartDate)
            .WithErrorMessage("Policy can only be sold up to 60 days in advance.");
    }

    [Fact]
    public void SalesRequest_should_be_invalid_when_policy_has_no_holders()
    {
        // arrange
        var validSalesRequest = _faker.NoHolders();
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(validSalesRequest);

        // assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(r => r.PolicyHolders)
            .WithErrorMessage("Policy must have at least 1 Policy Holder.");
    }

    [Fact]
    public void SalesRequest_should_be_invalid_when_policy_has_more_than_3_holders()
    {
        // arrange
        var validSalesRequest = _faker.MoreThen3Holders();
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(validSalesRequest);

        // assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(r => r.PolicyHolders)
            .WithErrorMessage("Policy can only have up to 3 Policy Holders.");
    }

    [Fact]
    public void SalesRequest_should_be_invalid_when_policy_has_a_holder_younger_then_16()
    {
        // arrange
        var validSalesRequest = _faker.YoungerThan16();
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(validSalesRequest);

        // assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor("PolicyHolders[1].DateOfBirth")
            .WithErrorMessage("Policy Holder must be over 16 years of age.");
    }

    [Fact]
    public void SalesRequest_should_be_invalid_when_policy_has_a_holder_just_under_16()
    {
        // arrange
        var validSalesRequest = _faker.JustUnder16();
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(validSalesRequest);

        // assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor("PolicyHolders[0].DateOfBirth")
            .WithErrorMessage("Policy Holder must be over 16 years of age.");
    }
}
