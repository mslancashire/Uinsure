using Tests.Uinsure.Core;
using Uinsure.Core.Models;

namespace Tests.Uinsure.Basic.Domain;

public class HouseholdPolicyTests
{
    [Fact]
    public void Expired_should_return_false_when_policy_is_still_running()
    {
        // arrange
        var policy = new HouseHoldPolicy { StartDate = DateOnly.Parse("2025-05-15") };

        // act
        var hasExpired = policy.Expired(Settings.ReferenceDate);

        // assert
        hasExpired.Should().BeFalse();
    }

    [Fact]
    public void Expired_should_return_true_when_policy_is_finished()
    {
        // arrange
        var policy = new HouseHoldPolicy { StartDate = DateOnly.Parse("2025-04-15") };

        // act
        var hasExpired = policy.Expired(Settings.ReferenceDate);

        // assert
        hasExpired.Should().BeTrue();
    }

    [Fact]
    public void WithinRenewalPeriod_should_return_true_when_end_within_30_days()
    {
        // arrange
        var policy = new HouseHoldPolicy { StartDate = DateOnly.Parse("2025-06-15") };

        // act
        var withinRenewPeriod = policy.WithinRenewalPeriod(Settings.ReferenceDate);

        // assert
        withinRenewPeriod.Should().BeTrue();
    }

    [Fact]
    public void WithinRenewalPeriod_should_return_false_when_outside_of_30_days()
    {
        // arrange
        var policy = new HouseHoldPolicy { StartDate = DateOnly.Parse("2025-05-15") };

        // act
        var withinRenewPeriod = policy.WithinRenewalPeriod(Settings.ReferenceDate);

        // assert
        withinRenewPeriod.Should().BeFalse();
    }
}
