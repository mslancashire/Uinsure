using Tests.Uinsure.Integration.Setup;

namespace Tests.Uinsure.Integration;

public class PolicySaleTests : BaseIntegrationTestFixture
{
    public PolicySaleTests(CustomWebApplicationFactory application) : base(application)
    {
    }

    [Fact]
    public async Task PolicySale_should_return_201_when_valid()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var endpoint = "api/v1/policy";
        var content = new StringContent(string.Empty, System.Text.Encoding.UTF8, "application/json");

        // act
        var response = await _client.PostAsync(endpoint, content, cancellationToken);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
        var policyId = response.Headers.Location?.Segments.LastOrDefault();
        policyId.Should().Satisfy<string>(x => Guid.TryParse(x, out _));
    }

    [Fact]
    public async Task PolicyDetails_should_return_200_with_correct_details()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var policyId = Guid.NewGuid();
        var endpoint = $"api/v1/policy/{policyId}";

        // act
        var response = await _client.GetAsync(endpoint, cancellationToken);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
