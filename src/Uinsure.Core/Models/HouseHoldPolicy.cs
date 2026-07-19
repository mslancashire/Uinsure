namespace Uinsure.Core.Models;

public class HouseHoldPolicy
{
    private readonly List<Payment> _payments = [];
    
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
    public DateOnly StartDate { get; init; }

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
    /// Adds a payment to the policy.
    /// </summary>
    /// <param name="paymentType"></param>
    /// <param name="amount"></param>
    public HouseHoldPolicy MakePayment(PaymentType paymentType, decimal amount)
    {
        if (paymentType != PaymentType.None && amount > 0)
        {
            _payments.Add(Payment.Basic(paymentType, amount));
        }

        return this;
    }
}
