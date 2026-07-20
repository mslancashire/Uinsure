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
    public void Policy_should_handle_correctly_expiry_and_renewal_period_checks_with_various_start_dates(string startDateString, bool expectedWithin, bool expectedExpired)
    {
        // arrange
        var policy = new HouseHoldPolicy { StartDate = DateOnly.Parse(startDateString) };

        var daysDiff = policy.EndDate.DayNumber - Settings.ReferenceDate.DayNumber;
        _output.WriteLine($"Checking {policy.EndDate} against {Settings.ReferenceDate} ({daysDiff}).");

        // act
        var withinRenewPeriod = policy.WithinRenewalPeriod(Settings.ReferenceDate);
        var expired = policy.Expired(Settings.ReferenceDate);

        // assert
        withinRenewPeriod.Should().Be(expectedWithin);
        expired.Should().Be(expectedExpired);
    }

    // reference date (today date) => 2026-05-01

    [Theory]
    [InlineData("2026-03-15", true)] // past
    [InlineData("2026-04-30", true)] // past
    [InlineData("2026-05-01", true)] // same day
    [InlineData("2026-05-02", false)] // future
    [InlineData("2026-05-15", false)] // future
    [InlineData("2026-07-15", false)] // future
    public void Started_should_correctly_state_if_policy_has_started_with_various_start_dates(string startDateString, bool expectedIsStarted)
    {
        // arrange
        var policy = new HouseHoldPolicy { StartDate = DateOnly.Parse(startDateString) };

        _output.WriteLine($"Checking {policy.StartDate} against {Settings.ReferenceDate}.");

        // act
        var hasPolicyStarted = policy.Started(Settings.ReferenceDate);

        // assert
        hasPolicyStarted.Should().Be(expectedIsStarted);
    }

    // reference date (today date) => 2026-05-01

    [Theory]
    [InlineData("2026-03-15", false)] // past, 47 days
    [InlineData("2026-04-15", false)] // past, 16 days
    [InlineData("2026-04-17", true)] // past, 14 days
    [InlineData("2026-04-28", true)] // past, 3 days
    [InlineData("2026-04-30", true)] // past, 1 day
    [InlineData("2026-05-01", true)] // same day
    [InlineData("2026-05-02", false)] // future, -1 day
    [InlineData("2026-05-14", false)] // future, -13 days
    [InlineData("2026-05-15", false)] // future, -14 days
    [InlineData("2026-07-15", false)] // future, -75 days
    public void WithinCoolingOffPeriod_should_correctly_state_if_policy_is_within_cooling_off_period_with_various_start_dates(string startDateString, bool expectedIsStarted)
    {
        // arrange
        var policy = new HouseHoldPolicy { StartDate = DateOnly.Parse(startDateString) };

        var daysDiff = Settings.ReferenceDate.DayNumber - policy.StartDate.DayNumber;
        _output.WriteLine($"Checking {policy.StartDate} against {Settings.ReferenceDate} ({daysDiff}).");

        // act
        var hasPolicyStarted = policy.WithinCoolingOffPeriod(Settings.ReferenceDate);

        // assert
        hasPolicyStarted.Should().Be(expectedIsStarted);
    }

    [Fact]
    public void CalculateRefund_should_output_no_refund_when_there_are_no_payments()
    {
        // arrange
        var policy = new HouseHoldPolicy { StartDate = Settings.ReferenceDate };

        // act
        var refund = policy.CalculateRefund(Settings.ReferenceDate);

        // assert
        refund.Should().BeOfType(typeof(NoPayment));
    }

    [Theory]
    [InlineData("2024-03-15", typeof(NoRefund))] // past, expired
    [InlineData("2026-03-15", typeof(ProRataRefund))] // past, started outside window
    [InlineData("2026-04-17", typeof(FullRefund))] // past, started inside window
    [InlineData("2026-05-01", typeof(FullRefund))] // same day, started inside window
    [InlineData("2026-05-02", typeof(FullRefund))] // future, not started
    [InlineData("2026-08-02", typeof(FullRefund))] // future, not started
    public void CalculateRefund_should_correctly_output_the_right_refund_type_based_on_various_start_dates(string startDateString, Type expectedRefund)
    {
        // arrange
        var saleRequest = new PolicySaleRequest { StartDate = DateOnly.Parse(startDateString), PaymentType = PaymentType.Cheque, Price = 250M };
        var policy = HouseHoldPolicy.CreateNewSale(saleRequest);

        var daysDiff = Settings.ReferenceDate.DayNumber - policy.StartDate.DayNumber;
        _output.WriteLine($"Checking {policy.StartDate} against {Settings.ReferenceDate} ({daysDiff}).");

        // act
        var refund = policy.CalculateRefund(Settings.ReferenceDate);

        // assert
        refund.Should().BeOfType(expectedRefund);
    }

    [Theory]
    [InlineData("2024-03-15", false)] // past, expired
    [InlineData("2026-03-15", true)] // past, started outside window
    [InlineData("2026-04-17", true)] // past, started inside window
    [InlineData("2026-05-01", true)] // same day, started inside window
    [InlineData("2026-05-02", true)] // future, not started
    [InlineData("2026-08-02", true)] // future, not started
    public void Cancel_should_set_policy_cancellation_state_correctly_based_on_various_start_date(string startDateString, bool expectedCancellationState)
    {
        // arrange
        var saleRequest = new PolicySaleRequest { StartDate = DateOnly.Parse(startDateString), PaymentType = PaymentType.Cheque, Price = 250M };
        var policy = HouseHoldPolicy.CreateNewSale(saleRequest);

        // act
        policy.Cancel(Settings.ReferenceDate);

        // assert
        policy.HasBeenCancelled.Should().Be(expectedCancellationState);
        if (expectedCancellationState)
        {
            policy.Refunds.Should().HaveCount(1);
        }
    }
}
