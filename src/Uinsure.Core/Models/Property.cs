namespace Uinsure.Core.Models;

public class Property
{
    public static Property Basic(string addressLine1, string postCode)
        => new() { AddressLine1 = addressLine1, Postcode = postCode };

    public string AddressLine1 { get; init; } = string.Empty;

    public string AddressLine2 { get; init; } = string.Empty;

    public string AddressLine3 { get; init; } = string.Empty;

    public string Postcode { get; init; } = string.Empty;
}
