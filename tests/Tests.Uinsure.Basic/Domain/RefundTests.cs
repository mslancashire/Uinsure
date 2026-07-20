using Uinsure.Core.Models;

namespace Tests.Uinsure.Basic.Domain;

public class RefundTests
{
    [Fact]
    public void FullRefund_creation_from_payment_should_be_correct()
    {
        // arrange
        var payment = Payment.Basic(PaymentType.Cheque, 259.99M);

        // act
        var refund = FullRefund.Create(payment);

        // assert
        refund.Should().NotBeNull();
        refund.Reference.Should().NotBeEmpty();
        refund.Reference.Should().NotBe(payment.Reference);
        refund.Amount.Should().Be(payment.Amount);
        refund.OriginalPayment.Should().Be(payment);
    }

    [Theory]
    [InlineData(365, 20, 345)]
    [InlineData(259.99, 50, 224.37)]
    [InlineData(70.99, 364, 0.19)]
    public void ProRataRefund_creation_from_payment_and_days_used_should_be_correct(decimal policyAmount, int daysUsed, decimal expectedRefund)
    {
        // arrange
        var payment = Payment.Basic(PaymentType.Cheque, policyAmount);

        // act
        var refund = ProRataRefund.Create(payment, daysUsed);

        // assert
        refund.Should().NotBeNull();
        refund.Reference.Should().NotBeEmpty();
        refund.Reference.Should().NotBe(payment.Reference);
        refund.OriginalPayment.Should().Be(payment);

        refund.Amount.Should().Be(expectedRefund);
    }
}
