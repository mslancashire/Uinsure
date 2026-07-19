using System.Text;
using Tests.Uinsure.Core;
using Tests.Uinsure.Core.Fakes;
using Tests.Uinsure.Integration.Helpers;
using Tests.Uinsure.Integration.Setup;
using Uinsure.Core.Models;

namespace Tests.Uinsure.Integration;

public class PolicyRenewalTests : BaseIntegrationTestFixture
{
    private readonly FakePolicySaleRequests _faker;

    public PolicyRenewalTests(CustomWebApplicationFactory application) : base(application)
    {
        _faker = new FakePolicySaleRequests(Settings.TimeProvider);
    }

    [Fact]
    public async Task PolicyRenewal_should_return_200_for_a_successful_renewal()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var expectedPolicy = FakeHouseholdPolicies.Existing;
        var content = new StringContent(null, Encoding.UTF8, "application/json");
        var endpoint = $"api/v1/policy/{expectedPolicy.Reference}/renew";

        // act
        var response = await _client.PostAsync(endpoint, content, cancellationToken);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var foundPolicy = await response.GetAs<HouseHoldPolicy>();
        foundPolicy.Should().NotBeNull();
        foundPolicy.Reference.Should().Be(expectedPolicy.Reference);
        foundPolicy.StartDate.Should().Be(expectedPolicy.StartDate);
        foundPolicy.EndDate.Should().Be(expectedPolicy.EndDate);
    }
}
