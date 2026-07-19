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
}
