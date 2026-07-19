using Uinsure.Core.Models;

namespace Tests.Uinsure.Core.Fakes;

public class FakeProperties
{
    public static Property Valid()
        => Property.Basic("123 Main Street", "SW1A 1AA");

    public static Property MissingAddressLine1()
        => Property.Basic(string.Empty, "SW1A 1AA");

    public static Property MissingPostcode()
        => Property.Basic("123 Main Street", string.Empty);

    public static Property PostcodeTooLong()
        => Property.Basic("123 Main Street", "123456789");
}
