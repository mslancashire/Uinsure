using Tests.Uinsure.Core;
using Uinsure.Core.Models;

namespace Tests.Uinsure.Basic.Domain;

public class HouseholdPolicyTests
{
    private readonly ITestOutputHelper _output;

    public HouseholdPolicyTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void PolicyLengthInDays_should_return_365_for_one_year_policy()
    {
        // arrange
        var policy = new HouseHoldPolicy { StartDate = DateOnly.Parse("2025-05-15") };

        // act
        var policyLength = policy.PolicyLengthInDays;

        // assert
        policyLength.Should().Be(365);
    }

    [Fact]
    public void Renew_should_create_new_payment_when_auto_renew_enabled()
    {
        // arrange
        var saleRequest = new PolicySaleRequest
        {
            StartDate = DateOnly.Parse("2025-05-15"),
            PaymentType = PaymentType.Card,
            Price = 150.00M
        };
        var policy = HouseHoldPolicy.CreateNewSale(saleRequest);
        var initialPaymentCount = policy.Payments.Count();

        // act
        policy.Renew();

        // assert
        policy.AutoRenew.Should().BeTrue();
        policy.Payments.Count().Should().Be(initialPaymentCount + 1);
        policy.StartDate.Should().Be(DateOnly.Parse("2026-05-15"));
    }

    [Fact]
    public void Renew_should_not_create_payment_when_auto_renew_disabled()
    {
        // arrange - Create policy without making a payment
        var policy = new HouseHoldPolicy { StartDate = DateOnly.Parse("2025-05-15") };
        var initialPaymentCount = policy.Payments.Count();

        // act
        policy.Renew();

        // assert
        policy.AutoRenew.Should().BeFalse();
        policy.Payments.Count().Should().Be(initialPaymentCount);
        policy.StartDate.Should().Be(DateOnly.Parse("2026-05-15"));
    }

    // reference date (today date) => 2026-05-01

    [Theory]
    [InlineData("2025-04-15", true, true)] // -16 days - within renewal, expired
    [InlineData("2025-04-30", true, true)] // -1 days - within renewal, expired
    [InlineData("2025-05-01", true, false)] // 0 days - within renewal, not expired
    [InlineData("2025-05-02", true, false)] // 1 days - within renewal, not expired
    [InlineData("2025-05-15", true, false)] // 14 days - within review, not expired
    [InlineData("2025-05-30", true, false)] // 29 days - within renewal, not expired
    [InlineData("2025-05-31", true, false)]  // 30 days - within review, not expired
    [InlineData("2025-06-01", false, false)]  // 31 days - outside renewal, not expired
    [InlineData("2025-06-15", false, false)]  // 45 days - outside renewal, not expired
    public void Policy_should_handle_various_start_dates(string startDateString, bool expectedWithin, bool expectedExpired)
    {
        // arrange
        var policy = new HouseHoldPolicy { StartDate = DateOnly.Parse(startDateString) };

        var daysDiff = policy.EndDate.DayNumber - Settings.ReferenceDate.DayNumber;
        _output.WriteLine($"Checking {policy.EndDate} against {Settings.ReferenceDate} ({daysDiff})");

        // act
        var withinRenewPeriod = policy.WithinRenewalPeriod(Settings.ReferenceDate);
        var expired = policy.Expired(Settings.ReferenceDate);

        // assert
        withinRenewPeriod.Should().Be(expectedWithin);
        expired.Should().Be(expectedExpired);
    }
}
