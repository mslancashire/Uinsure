namespace Uinsure.Core.Models;

public class HouseHoldPolicy
{
    private readonly List<Payment> _payments = [];
    private readonly List<Refund> _refunds = [];
    
    public static HouseHoldPolicy CreateNewSale(PolicySaleRequest saleRequest)
    {
        var policy = new HouseHoldPolicy
        {
            Reference = Guid.NewGuid(),
            StartDate = saleRequest.StartDate,
            PolicyHolders = saleRequest.PolicyHolders,
            Property = saleRequest.Property,
        };

        policy.MakePayment(saleRequest.PaymentType, saleRequest.Price);

        return policy;
    }

    /// <summary>
    /// Using a guid for a policy reference to ensure uniqueness and avoid collisions.
    /// However if the requirements change in order that it be provided, e.g. from an Insurance provider, then this can be changed to a string and the value can be provided from the provider.
    /// You could also continue to use a guid but also have a string reference that is provided by the provider.
    /// You could also consider using a textual identifier that is more human readable for quotation over the phone etc...
    /// </summary>
    public Guid Reference { get; init; }

    /// <summary>
    /// Start date of the policy.
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// End date of the policy, which is one year after the start date.
    /// </summary>
    public DateOnly EndDate => StartDate.AddYears(1);

    public int PolicyLengthInDays => EndDate.DayNumber - StartDate.DayNumber;

    /// <summary>
    /// The policy holders.
    /// </summary>
    public IEnumerable<PolicyHolder> PolicyHolders { get; init; } = [];

    /// <summary>
    /// The property the policy covers.
    /// </summary>
    public Property? Property { get; init; }

    /// <summary>
    /// Payments made on the policy.
    /// </summary>
    public IEnumerable<Payment> Payments => _payments;

    /// <summary>
    /// Refunds made on the policy.
    /// </summary>
    public IEnumerable<Refund> Refunds => _refunds;

    /// <summary>
    /// Adds a payment to the policy.
    /// </summary>
    /// <param name="paymentType"></param>
    /// <param name="amount"></param>
    public HouseHoldPolicy MakePayment(PaymentType paymentType, decimal amount)
        => MakePayment(Payment.Basic(paymentType, amount));

    private HouseHoldPolicy MakePayment(Payment payment)
    {
        if (payment.Type != PaymentType.None && payment.Amount > 0)
        {
            _payments.Add(payment);
        }

        return this;
    }

    /// <summary>
    /// Auto Renew is enabled if there are payments.
    /// </summary>
    public bool AutoRenew => Payments.Any();

    /// <summary>
    /// States if the policy has expired.
    /// </summary>
    /// <param name="now"></param>
    /// <returns></returns>
    public bool Expired(DateOnly now)
        => EndDate < now;

    /// <summary>
    /// States if the policy is withing the 30 day renewal period.
    /// </summary>
    /// <param name="now"></param>
    /// <returns></returns>
    public bool WithinRenewalPeriod(DateOnly now)
        => (EndDate.DayNumber - now.DayNumber) <= 30;

    /// <summary>
    /// Renews the policy, adding a payment if Auto Renew is enabled.
    /// </summary>
    /// <returns></returns>
    public HouseHoldPolicy Renew()
    {
        StartDate = StartDate.AddYears(1);
        
        if (AutoRenew)
        {
            MakePayment(Payment.Basic(Payments.First()));
        }
        
        return this;
    }

    /// <summary>
    /// States if the policy has been cancelled or not.
    /// </summary>
    public bool HasBeenCancelled { get; set; } = false;

    /// <summary>
    /// Cancels the policy applying the correct refund.
    /// </summary>
    /// <param name="now"></param>
    public void Cancel(DateOnly now)
    {
        if (HasBeenCancelled)
        {
            return;
        }
        
        var refund = CalculateRefund(now);

        if (refund is FullRefund || refund is ProRataRefund)
        {
            HasBeenCancelled = true;
            _refunds.Add(refund);
        }
    }

    /// <summary>
    /// States if the policy has started or not.
    /// </summary>
    /// <param name="now"></param>
    /// <returns></returns>
    internal bool Started(DateOnly now)
        => StartDate <= now;

    /// <summary>
    /// States if the policy has started and if its still withing the cooling off period.
    /// </summary>
    /// <param name="now"></param>
    /// <returns></returns>
    internal bool WithinCoolingOffPeriod(DateOnly now)
        => Started(now) && (now.DayNumber - StartDate.DayNumber) <= 14;

    /// <summary>
    /// States how many of the days the policy has been used for.
    /// </summary>
    /// <param name="now"></param>
    /// <returns></returns>
    internal int DaysUsed(DateOnly now)
    {
        if (Started(now) == false)
        {
            return 0;
        }
        
        if (Expired(now))
        {
            return EndDate.DayNumber - StartDate.DayNumber;
        }

        return now.DayNumber - StartDate.DayNumber;
    }

    /// <summary>
    /// Calculates a refund if the policy was to be cancelled.
    /// </summary>
    /// <param name="now"></param>
    /// <returns></returns>
    public Refund CalculateRefund(DateOnly now)
    {
        if (Payments.Any() == false)
        {
            return NoPayment.Create();
        }

        if (Expired(now))
        {
            return NoRefund.Create();
        }

        if (Started(now) == false)
        {
            return FullRefund.Create(Payments.First());
        }

        if (WithinCoolingOffPeriod(now))
        {
            return FullRefund.Create(Payments.First());
        }

        return ProRataRefund.Create(Payments.First(), DaysUsed(now));
    }
}
