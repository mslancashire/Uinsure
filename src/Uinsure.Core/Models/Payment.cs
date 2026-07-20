namespace Uinsure.Core.Models;

public record Payment
{
    public static Payment Empty()
        => new() { Reference = Guid.Empty, Type = PaymentType.None, Amount = 0 };

    public static Payment Basic(PaymentType paymentType, decimal amount)
        => new() { Reference = Guid.NewGuid(), Type = paymentType, Amount = amount };

    public static Payment Basic(Payment payment)
        => new() { Reference = Guid.NewGuid(), Type = payment.Type, Amount = payment.Amount };

    public Guid Reference { get; init; }

    public PaymentType Type { get; init; }

    public decimal Amount { get; init; }
}
