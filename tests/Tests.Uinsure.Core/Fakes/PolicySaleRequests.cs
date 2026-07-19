using Uinsure.Core.Models.PolicySale;

namespace Tests.Uinsure.Core.Fakes;

public class PolicySaleRequests
{
    public static PolicySaleRequest MissingStartDate()
        => new();

    public static PolicySaleRequest Valid()
        => Valid(DateTimeOffset.UtcNow);

    public static PolicySaleRequest Valid(DateTimeOffset now)
        => new() { StartDate = DateOnly.FromDateTime(now.AddDays(5).Date) };

    public static PolicySaleRequest ForToday(DateTimeOffset now)
        => new() { StartDate = DateOnly.FromDateTime(now.Date) };

    public static PolicySaleRequest InPast(DateTimeOffset now)
        => new() { StartDate = DateOnly.FromDateTime(now.AddDays(-1).Date) };

    public static PolicySaleRequest Over60Days(DateTimeOffset now)
        => new() { StartDate = DateOnly.FromDateTime(now.AddDays(62).Date) };

    public static PolicySaleRequest Exactly60Days(DateTimeOffset now)
        => new() { StartDate = DateOnly.FromDateTime(now.AddDays(60).Date) };
}
