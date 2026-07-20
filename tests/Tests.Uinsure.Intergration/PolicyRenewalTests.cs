using Microsoft.AspNetCore.Http;
using System.Text;
using Tests.Uinsure.Core;
using Tests.Uinsure.Core.Fakes;
using Tests.Uinsure.Integration.Helpers;
using Tests.Uinsure.Integration.Setup;
using Uinsure.Core.Models;

namespace Tests.Uinsure.Integration;

public class PolicyRenewalTests : BaseIntegrationTestFixture
{   
    private readonly CancellationToken _cancellationToken;

    public PolicyRenewalTests(CustomWebApplicationFactory application) : base(application)
    {
        _cancellationToken = new CancellationTokenSource().Token;
    }

    private string RenewalEndpoint(HouseHoldPolicy policy)
        => $"/api/v1/policy/{policy.Reference}/renew";

    private async Task<HttpResponseMessage> PerformRenewal(HouseHoldPolicy policy)
    {
        var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
        return await _client.PutAsync(RenewalEndpoint(policy), content, _cancellationToken);
    }


    [Fact]
    public async Task PolicyRenewal_should_return_200_for_a_successful_renewal_with_no_payments()
    {
        // arrange
        var expectedPolicy = FakeHouseholdPolicies.ExistingWithoutPayments;

        // act
        var response = await PerformRenewal(expectedPolicy);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var foundPolicy = await response.GetAs<HouseHoldPolicy>();
        foundPolicy.Should().NotBeNull();
        foundPolicy.Reference.Should().Be(expectedPolicy.Reference);
        foundPolicy.StartDate.Should().Be(Settings.DateForRenewal.AddYears(1));
        foundPolicy.EndDate.Should().Be(Settings.DateForRenewal.AddYears(2));
        foundPolicy.Payments.Should().HaveCount(0);
    }

    [Fact]
    public async Task PolicyRenewal_should_return_200_for_a_successful_renewal_with_payments()
    {
        // arrange
        var expectedPolicy = FakeHouseholdPolicies.ExistingWithPayments;

        // act
        var response = await PerformRenewal(expectedPolicy);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var foundPolicy = await response.GetAs<HouseHoldPolicy>();
        foundPolicy.Should().NotBeNull();
        foundPolicy.Reference.Should().Be(expectedPolicy.Reference);
        foundPolicy.StartDate.Should().Be(Settings.DateForRenewal.AddYears(1));
        foundPolicy.EndDate.Should().Be(Settings.DateForRenewal.AddYears(2));
        foundPolicy.Payments.Should().HaveCount(2);

        var firstPayment = foundPolicy.Payments.ElementAt(0);
        var renewalPayment = foundPolicy.Payments.ElementAt(1);

        renewalPayment.Reference.Should().NotBe(firstPayment.Reference);
        renewalPayment.Type.Should().Be(firstPayment.Type);
        renewalPayment.Amount.Should().Be(firstPayment.Amount);
    }

    [Fact]
    public async Task PolicyRenewal_should_return_404_for_renew_when_policy_is_not_found()
    {
        // arrange
        var expectedPolicy = FakeHouseholdPolicies.MissingPolicy;

        // act
        var response = await PerformRenewal(expectedPolicy);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PolicyRenewal_should_return_400_for_renew_when_policy_is_expired()
    {
        // arrange
        var expectedPolicy = FakeHouseholdPolicies.Expired;

        // act
        var response = await PerformRenewal(expectedPolicy);

        // assert
        await CheckForProblem(response, expectedPolicy, "Policy can not be renewed after the policy end date.");
    }

    [Fact]
    public async Task PolicyRenewal_should_return_400_for_renew_when_policy_is_not_within_renewal_window()
    {
        // arrange
        var expectedPolicy = FakeHouseholdPolicies.OutsideRenewalPeriod;

        // act
        var response = await PerformRenewal(expectedPolicy);

        // assert
        await CheckForProblem(response, expectedPolicy, "Policy can only be renewed 30 days before end date.");
    }

    private async Task CheckForProblem(HttpResponseMessage response, HouseHoldPolicy expectedPolicy, string errorMessage)
    {
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problemDetails = await response.GetAs<HttpValidationProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails.Status.Should().Be((int)HttpStatusCode.BadRequest);
        problemDetails.Title.Should().Be("Validation failed");
        problemDetails.Detail.Should().Be("One or more validation errors occurred.");
        problemDetails.Instance.Should().Be(RenewalEndpoint(expectedPolicy));
        problemDetails.Errors.Should().HaveCount(1);
        problemDetails.Errors["EndDate"].Should().HaveCount(1);
        problemDetails.Errors["EndDate"].First().Should().Be(errorMessage);
    }
}
