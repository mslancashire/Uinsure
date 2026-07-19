using FluentValidation.TestHelper;
using Tests.Uinsure.Core.Fakes;
using Uinsure.Core.Validators;

namespace Tests.Uinsure.Basic.Validation;

public class PropertyValidatorTests
{
    private readonly FakeProperties _faker = new();

    private PropertyValidator CreateValidator()
        => new();

    [Fact]
    public void Property_should_be_valid()
    {
        // arrange
        var property = FakeProperties.Valid();
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(property);

        // assert
        result.IsValid.Should().BeTrue();
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Property_should_be_invalid_when_address_line_1_is_not_provided()
    {
        // arrange
        var property = FakeProperties.MissingAddressLine1();
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(property);

        // assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(r => r.AddressLine1)
            .WithErrorMessage("Must provide address line 1.");
    }

    [Fact]
    public void Property_should_be_invalid_when_postcode_is_not_provided()
    {
        // arrange
        var property = FakeProperties.MissingPostcode();
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(property);

        // assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(r => r.Postcode)
            .WithErrorMessage("Must provide postcode.");
    }

    [Fact]
    public void Property_should_be_invalid_when_postcode_is_longer_than_8_characters()
    {
        // arrange
        var property = FakeProperties.PostcodeTooLong();
        var validator = CreateValidator();

        // act
        var result = validator.TestValidate(property);

        // assert
        result.IsValid.Should().BeFalse();
        result.ShouldHaveValidationErrorFor(r => r.Postcode)
            .WithErrorMessage("Postcode must not be longer than 8 characters.");
    }
}
