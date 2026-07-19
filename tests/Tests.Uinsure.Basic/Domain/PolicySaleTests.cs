using Tests.Uinsure.Core;
using Tests.Uinsure.Core.Fakes;
using Uinsure.Core.Models;

namespace Tests.Uinsure.Basic.Domain;

public class PolicySaleTests
{
    private readonly FakePolicySaleRequests _faker;

    public PolicySaleTests()
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
        policy.Should().NotBeNull();
        policy.PolicyLengthInDays.Should().Be(365);
    }

    [Fact]
    public void PolicyHolders_should_reflect_those_provided_on_the_sale_request()
    {
        // arrange
        var saleRequest = _faker.Valid();

        // act
        var policy = HouseHoldPolicy.CreateNewSale(saleRequest);

        // assert
        policy.Should().NotBeNull();
        policy.PolicyHolders.Should().HaveCount(1);
    }

    [Fact]
    public void Property_should_reflect_the_one_on_the_sale_request()
    {
        // arrange
        var saleRequest = _faker.Valid();

        // act
        var policy = HouseHoldPolicy.CreateNewSale(saleRequest);

        // assert
        policy.Should().NotBeNull();
        policy.Property.Should().Be(saleRequest.Property);
    }

    [Fact]
    public void Payments_should_reflect_the_payment_provided_on_the_sale_request()
    {
        // arrange
        var saleRequest = _faker.ValidWithPayment();

        // act
        var policy = HouseHoldPolicy.CreateNewSale(saleRequest);

        // assert
        policy.Should().NotBeNull();
        policy.Payments.Should().HaveCount(1);
        
        var payment = policy.Payments.ElementAt(0);
        payment.Reference.Should().NotBeEmpty();
        payment.Type.Should().Be(saleRequest.PaymentType);
        payment.Amount.Should().Be(saleRequest.Price);
    }
}
