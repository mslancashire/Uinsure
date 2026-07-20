using Uinsure.Core.Models;

namespace Tests.Uinsure.Core.Fakes;

public class FakeHouseholdPolicies
{
    public static IEnumerable<HouseHoldPolicy> ExistingPolicies =>
        [ExistingWithoutPayments, ExistingWithPayments, Expired, OutsideRenewalPeriod];

    public static readonly HouseHoldPolicy ExistingWithoutPayments
        = HouseHoldPolicy.CreateNewSale(new PolicySaleRequest { StartDate = Settings.DateForRenewal });

    public static readonly HouseHoldPolicy ExistingWithPayments
        = HouseHoldPolicy.CreateNewSale(new PolicySaleRequest
        {
            StartDate = Settings.DateForRenewal,
            PaymentType = PaymentType.Card,
            Price = 159.59M
        });

    public static readonly HouseHoldPolicy Expired
        = HouseHoldPolicy.CreateNewSale(new PolicySaleRequest { StartDate = Settings.ReferenceDate.AddMonths(-13) });

    public static readonly HouseHoldPolicy OutsideRenewalPeriod
        = HouseHoldPolicy.CreateNewSale(new PolicySaleRequest { StartDate = Settings.ReferenceDate.AddMonths(-11) });

    public static readonly HouseHoldPolicy MissingPolicy = new() { Reference = Guid.NewGuid() };
}
