using Tests.Uinsure.Integration.Setup;

namespace Tests.Uinsure.Integration;

public class PolicySaleTests : BaseIntegrationTestFixture
{
    public PolicySaleTests(CustomWebApplicationFactory application) : base(application)
    {
    }

    [Fact]
    public async Task PolicySale_should_return_201_when_valid_and_200_for_the_details()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var endpoint = "api/v1/policy";
        var content = new StringContent(string.Empty, System.Text.Encoding.UTF8, "application/json");

        // act
        var createdResponse = await _client.PostAsync(endpoint, content, cancellationToken);

        // assert
        createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        createdResponse.Headers.Location.Should().NotBeNull();
        var policyId = createdResponse.Headers.Location?.Segments.LastOrDefault();
        policyId.Should().Satisfy<string>(x => Guid.TryParse(x, out _));

        // act
        var detailsResponse = await _client.GetAsync(createdResponse.Headers.Location, cancellationToken);
        detailsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PolicyDetails_should_return_200_for_an_existing_policy()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var policyId = Fakes.HouseholdPolicies.Existing.Reference;
        var endpoint = $"api/v1/policy/{policyId}";

        // act
        var response = await _client.GetAsync(endpoint, cancellationToken);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
