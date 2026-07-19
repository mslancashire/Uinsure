using Uinsure.Core.Models;

namespace Tests.Uinsure.Basic.Domain;

public class PolicyEndDateTests
{
    [Fact]
    public void EndDate_should_reflect_a_one_policy_length_period()
    {
        // arrange
        var saleRequest = Fakes.PolicySaleRequests.Valid();

        // act
        var policy = HouseHoldPolicy.CreateNewSale(saleRequest);

        // assert
        policy.PolicyLengthInDays.Should().Be(365);
    }
}
