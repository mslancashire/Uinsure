using Uinsure.Core.Models.PolicySale;

namespace Uinsure.Core.Models;

public class HouseHoldPolicy
{
    public static HouseHoldPolicy CreateNewSale(PolicySaleRequest saleRequest)
    {
        return new HouseHoldPolicy
        {
            Reference = Guid.NewGuid(),
            StartDate = saleRequest.StartDate,
            PolicyHolders = saleRequest.PolicyHolders,
        };
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
    public IEnumerable<PolicyHolder> PolicyHolders { get; init; }
}
