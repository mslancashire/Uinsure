using Uinsure.Core.Models;

namespace Tests.Uinsure.Core.Fakes;

public class FakeHouseholdPolicies
{
    public static HouseHoldPolicy Existing
        = HouseHoldPolicy.CreateNewSale(new PolicySaleRequest { StartDate = new DateOnly(2025, 06, 15) });

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
