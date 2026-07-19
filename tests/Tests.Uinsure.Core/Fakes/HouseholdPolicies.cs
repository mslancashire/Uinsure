using Uinsure.Core.Models;
using Uinsure.Core.Models.PolicySale;

namespace Tests.Uinsure.Core.Fakes;

public class HouseholdPolicies
{
    public static HouseHoldPolicy Existing
        = HouseHoldPolicy.CreateNewSale(new PolicySaleRequest { StartDate = new DateOnly(2026, 05, 15) });

    public static HouseHoldPolicy Valid
        = HouseHoldPolicy.CreateNewSale(new PolicySaleRequest { StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(59).Date) });
}
