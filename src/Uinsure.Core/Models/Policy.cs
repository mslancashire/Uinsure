namespace Uinsure.Core.Models;

public class HouseHoldPolicy
{
    public static HouseHoldPolicy CreateNewSale()
    {
        return new HouseHoldPolicy { Reference = Guid.NewGuid() };
    }

    /// <summary>
    /// Using a guid for a policy reference to ensure uniqueness and avoid collisions.
    /// However if the requirements change in order that it be provided, e.g. from an Insurance provider, then this can be changed to a string and the value can be provided from the provider.
    /// You could also continue to use a guid but also have a string reference that is provided by the provider.
    /// You may also want to consider using a textual identifier that is more human readable for quotation over the phone etc...
    /// </summary>
    public Guid Reference { get; private set; }
}
