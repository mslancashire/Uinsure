using Uinsure.Core.Models;

namespace Tests.Uinsure.Core.Fakes;

public class FakePolicyHolders
{
    public static PolicyHolder Over16(DateOnly reference)
        => PolicyHolder.Basic("Over", "16", reference.AddYears(-17));

    public static PolicyHolder JustUnder16(DateOnly reference)
        => PolicyHolder.Basic("Exactly", "16", reference.AddYears(-16).AddDays(15));

    public static PolicyHolder Under16(DateOnly reference)
        => PolicyHolder.Basic("Under", "16", reference.AddYears(-15));

    public static PolicyHolder MissingFirstName(DateOnly reference)
        => PolicyHolder.Basic(string.Empty, "Test", reference.AddYears(-17));

    public static PolicyHolder MissingLastName(DateOnly reference)
        => PolicyHolder.Basic("Test", string.Empty, reference.AddYears(-17));

    public static PolicyHolder MissingDateOfBirth()
        => new() { FirstName = "Test", LastName = "Test", DateOfBirth = default };
}
