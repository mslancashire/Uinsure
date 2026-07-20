using Microsoft.AspNetCore.Mvc;
using Tests.Uinsure.Core;
using Tests.Uinsure.Core.Fakes;
using Tests.Uinsure.Integration.Helpers;
using Tests.Uinsure.Integration.Setup;
using Uinsure.Core.Models;

namespace Tests.Uinsure.Integration;

public class PolicySaleTests : BaseIntegrationTestFixture
{
    private readonly FakePolicySaleRequests _faker;

    public PolicySaleTests(CustomWebApplicationFactory application) : base(application)
    {
        _faker = new FakePolicySaleRequests(Settings.TimeProvider);
    }

    [Fact]
    public async Task PolicySale_should_return_201_when_valid_and_200_for_the_details()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var endpoint = "api/v1/policy";
        var policySaleRequest = _faker.Valid();
        
        // act
        var createdResponse = await _client.PostAsync(endpoint, policySaleRequest.ToJsonContent(), cancellationToken);

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
        createdPolicy.StartDate.Should().Be(policySaleRequest.StartDate);
        createdPolicy.EndDate.Should().Be(policySaleRequest.StartDate.AddYears(1));
        createdPolicy.PolicyLengthInDays.Should().Be(365);
    }

    [Fact]
    public async Task PolicyDetails_should_return_200_for_an_existing_policy()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var expectedPolicy = Fakes.FakeHouseholdPolicies.ExistingWithoutPayments;
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
        var salesRequest = _faker.MissingStartDate();

        // act
        var response = await _client.PostAsync(endpoint, salesRequest.ToJsonContent() ,cancellationToken);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.GetAs<ProblemDetails>();
        problemDetails.Should().NotBeNull();
    }

    [Fact]
    public async Task PolicySale_should_return_400_when_no_policy_holders_are_provided()
    {
        // arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var endpoint = $"api/v1/policy";
        var salesRequest = _faker.MissingStartDate();

        // act
        var response = await _client.PostAsync(endpoint, salesRequest.ToJsonContent(), cancellationToken);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.GetAs<ProblemDetails>();
        problemDetails.Should().NotBeNull();
    }
}
