namespace Uinsure.Core.Models;

public record Refund
{
    internal Refund()
    {
        Reference = Guid.Empty;
        Amount = 0;
    }

    internal Refund(Payment payment, decimal amountToRefund)
    {
        Reference = Guid.NewGuid();
        OriginalPayment = payment;
        Amount = amountToRefund;
    }
    
    public Guid Reference { get; init; }

    public Payment? OriginalPayment { get; init; }

    public decimal Amount { get; init; }

    public string Type => GetType().Name;
}

public record NoPayment : Refund
{
    public NoPayment() : base()
    { }

    internal static NoPayment Create()
        => new();
}

public record NoRefund : Refund
{
    public NoRefund() : base()
    { }

    internal static NoRefund Create()
        => new();
}

public record FullRefund : Refund
{   
    public FullRefund(Payment payment, decimal amountToRefund) : base(payment, amountToRefund)
    {
    }

    internal static FullRefund Create(Payment payment)
        => new(payment, payment.Amount);
}

public record ProRataRefund : Refund
{
    public ProRataRefund(Payment payment, decimal amountToRefund) : base(payment, amountToRefund)
    {
    }

    internal static ProRataRefund Create(Payment payment, int daysUsed)
    {
        decimal proRataValue = (daysUsed / 365M) * payment.Amount;
        decimal valueLeft = payment.Amount - proRataValue;
        decimal roundedValue = Math.Round(valueLeft, 2, MidpointRounding.AwayFromZero);

        return new(payment, roundedValue);
    }
}
