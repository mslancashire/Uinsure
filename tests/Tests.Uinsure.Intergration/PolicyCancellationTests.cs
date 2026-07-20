using System.Text;
using Tests.Uinsure.Core.Fakes;
using Tests.Uinsure.Integration.Helpers;
using Tests.Uinsure.Integration.Setup;
using Uinsure.Core.Models;

namespace Tests.Uinsure.Integration;

public class PolicyCancellationTests : BaseIntegrationTestFixture
{
    private readonly CancellationToken _cancellationToken;

    public PolicyCancellationTests(CustomWebApplicationFactory application) : base(application)
    {
        _cancellationToken = new CancellationTokenSource().Token;
    }

    private string CancellationEndpoint(HouseHoldPolicy policy)
        => $"/api/v1/policy/{policy.Reference}/cancel";

    private string RefundCalculationEndpoint(HouseHoldPolicy policy)
        => $"/api/v1/policy/{policy.Reference}/refund";

    private async Task<HttpResponseMessage> PerformCancellation(HouseHoldPolicy policy)
    {
        var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
        return await _client.PutAsync(CancellationEndpoint(policy), content, _cancellationToken);
    }

    private async Task<HttpResponseMessage> PerformRefundCalculation(HouseHoldPolicy policy)
    {
        return await _client.GetAsync(RefundCalculationEndpoint(policy), _cancellationToken);
    }

    [Fact]
    public async Task PolicyCancellation_should_return_200_for_a_successful_cancellation_with_a_refund()
    {
        // arrange
        var expectedPolicy = FakeHouseholdPolicies.ToBeCancelled;

        // act
        var response = await PerformCancellation(expectedPolicy);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var foundPolicy = await response.GetAs<HouseHoldPolicy>();
        foundPolicy.Should().NotBeNull();
        foundPolicy.Reference.Should().Be(expectedPolicy.Reference);
        foundPolicy.HasBeenCancelled.Should().BeTrue();
        foundPolicy.Refunds.Should().HaveCount(1);
    }

    [Fact]
    public async Task PolicyCancellation_should_return_200_for_a_successful_cancellation_with_no_refund()
    {
        // arrange
        var expectedPolicy = FakeHouseholdPolicies.Expired;

        // act
        var response = await PerformCancellation(expectedPolicy);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var foundPolicy = await response.GetAs<HouseHoldPolicy>();
        foundPolicy.Should().NotBeNull();
        foundPolicy.Reference.Should().Be(expectedPolicy.Reference);
        foundPolicy.HasBeenCancelled.Should().BeFalse();
        foundPolicy.Refunds.Should().HaveCount(0);
    }

    [Fact]
    public async Task PolicyRefundCalculation_should_return_200_with_the_refund()
    {
        // arrange
        var expectedPolicy = FakeHouseholdPolicies.ToBeRefunded;

        // act
        var response = await PerformRefundCalculation(expectedPolicy);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var refund = await response.GetAs<Refund>();
        refund.Should().NotBeNull();
        refund.Reference.Should().NotBeEmpty();
        refund.Reference.Should().NotBe(expectedPolicy.Reference);
        refund.OriginalPayment.Should().NotBeNull();
        refund.Amount.Should().Be(6.12M);
    }
}
