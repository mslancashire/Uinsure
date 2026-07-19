using FluentValidation.TestHelper;
using Microsoft.Extensions.Time.Testing;
using Uinsure.Core.Models.PolicySale;

namespace Tests.Uinsure.Basic.Validation;

public class PolicySaleValidatorTests
{
    private readonly DateTimeOffset _baseDateTime = new(2026, 5, 1, 10, 0, 0, 0, TimeSpan.Zero);
    private readonly FakeTimeProvider _fakeTimeProvider =  new();

    public PolicySaleValidatorTests()
    {
        _fakeTimeProvider.SetUtcNow(_baseDateTime);
    }

    private PolicySaleValidator CreateValidator()
        => new(_fakeTimeProvider);

    [Fact]
    public void SalesRequest_should_be_valid()
    {
        // arrange
        var validSalesRequest = Fakes.PolicySaleRequests.Valid(_baseDateTime);
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
        var validSalesRequest = Fakes.PolicySaleRequests.ForToday(_baseDateTime);
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
        var validSalesRequest = Fakes.PolicySaleRequests.MissingStartDate();
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
        var validSalesRequest = Fakes.PolicySaleRequests.InPast(_baseDateTime);
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
        var validSalesRequest = Fakes.PolicySaleRequests.Over60Days(_baseDateTime);
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
        var validSalesRequest = Fakes.PolicySaleRequests.Exactly60Days(_baseDateTime);
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(validSalesRequest);

        // assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(r => r.StartDate)
            .WithErrorMessage("Policy can only be sold up to 60 days in advance.");
    }
}
