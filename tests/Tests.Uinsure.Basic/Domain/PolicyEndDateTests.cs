using Tests.Uinsure.Core;
using Tests.Uinsure.Core.Fakes;
using Uinsure.Core.Models;

namespace Tests.Uinsure.Basic.Domain;

public class PolicyEndDateTests
{
    private readonly FakePolicySaleRequests _faker;

    public PolicyEndDateTests()
    {
        _faker = new FakePolicySaleRequests(Settings.TimeProvider);
    }

    [Fact]
    public void EndDate_should_reflect_a_one_policy_length_period()
    {
        // arrange
        var saleRequest = _faker.Valid();

        // act
        var policy = HouseHoldPolicy.CreateNewSale(saleRequest);

        // assert
        policy.PolicyLengthInDays.Should().Be(365);
    }
}
