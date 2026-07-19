using Microsoft.AspNetCore.Mvc;
using Tests.Uinsure.Integration.Helpers;
using Tests.Uinsure.Integration.Setup;
using Uinsure.Core.Models;

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
        var expectedPolicy = Fakes.HouseholdPolicies.Valid;
        
        // act
        var createdResponse = await _client.PostAsync(endpoint, expectedPolicy.ToJsonContent(), cancellationToken);

        // assert
        createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        createdResponse.Headers.Location.Should().NotBeNull();
        
        var policyId = createdResponse.Headers.Location?.Segments.LastOrDefault();
        policyId.Should().Satisfy<string>(x => Guid.TryParse(x, out _));

        // act
        var detailsResponse = await _client.GetAsync(createdResponse.Headers.Location, cancellationToken);
        detailsResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createdPolicy = await detailsResponse.GetAs<HouseHoldPolicy>();
        createdPolicy.Should().NotBeNull();
        createdPolicy.Reference.Should().Be(policyId);
        createdPolicy.StartDate.Should().Be(expectedPolicy.StartDate);
        createdPolicy.EndDate.Should().Be(expectedPolicy.EndDate);
    }

    [Fact]
    public async Task PolicyDetails_should_return_200_for_an_existing_policy()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var expectedPolicy = Fakes.HouseholdPolicies.Existing;
        var endpoint = $"api/v1/policy/{expectedPolicy.Reference}";

        // act
        var response = await _client.GetAsync(endpoint, cancellationToken);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var foundPolicy = await response.GetAs<HouseHoldPolicy>();
        foundPolicy.Should().NotBeNull();
        foundPolicy.Reference.Should().Be(expectedPolicy.Reference);
        foundPolicy.StartDate.Should().Be(expectedPolicy.StartDate);
        foundPolicy.EndDate.Should().Be(expectedPolicy.EndDate);
    }

    [Fact]
    public async Task PolicySale_should_return_400_when_start_date_is_not_provided()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var endpoint = $"api/v1/policy";
        var salesRequest = Fakes.PolicySaleRequests.MissingStartDate();

        // act
        var response = await _client.PostAsync(endpoint, salesRequest.ToJsonContent() ,cancellationToken);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = response.GetAs<ProblemDetails>();
    }
}
