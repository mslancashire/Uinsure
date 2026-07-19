using Uinsure.Core.Models;
using Uinsure.Core.Models.PolicySale;

namespace Tests.Uinsure.Core.Fakes;

public class FakeHouseholdPolicies
{
    public static HouseHoldPolicy Existing
        = HouseHoldPolicy.CreateNewSale(new PolicySaleRequest { StartDate = new DateOnly(2026, 05, 15) });

    public static HouseHoldPolicy Valid
    {
        get
        {
            var saleRequest = new PolicySaleRequest()
            {
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(59).Date),
                PolicyHolders =
                [
                    PolicyHolder.Basic("Joe", "Blogs", new DateOnly(2000, 1 ,1))
                ]
            };

            return HouseHoldPolicy.CreateNewSale(saleRequest);
        }
    }
}
