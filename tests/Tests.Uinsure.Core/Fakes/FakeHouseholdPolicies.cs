using Uinsure.Core.Models;

namespace Tests.Uinsure.Core.Fakes;

public class FakeHouseholdPolicies
{
    public static IEnumerable<HouseHoldPolicy> ExistingPolicies =>
        [ExistingWithoutPayments, ExistingWithPayments, Expired, OutsideRenewalPeriod, ToBeCancelled, ToBeRefunded];

    public static readonly HouseHoldPolicy ExistingWithoutPayments
        = HouseHoldPolicy.CreateNewSale(new PolicySaleRequest { StartDate = Settings.DateForRenewal });

    public static readonly HouseHoldPolicy ExistingWithPayments
        = HouseHoldPolicy.CreateNewSale(new PolicySaleRequest
        {
            StartDate = Settings.DateForRenewal,
            PaymentType = PaymentType.Card,
            Price = 159.59M
        });

    public static readonly HouseHoldPolicy ToBeCancelled
        = HouseHoldPolicy.CreateNewSale(new PolicySaleRequest
        {
            StartDate = Settings.DateForRenewal,
            PaymentType = PaymentType.Card,
            Price = 159.59M
        });

    public static readonly HouseHoldPolicy ToBeRefunded
        = HouseHoldPolicy.CreateNewSale(new PolicySaleRequest
        {
            StartDate = Settings.DateForRenewal,
            PaymentType = PaymentType.Card,
            Price = 159.59M
        });

    public static readonly HouseHoldPolicy Expired
        = HouseHoldPolicy.CreateNewSale(new PolicySaleRequest { StartDate = Settings.ReferenceDate.AddMonths(-13) });

    public static readonly HouseHoldPolicy OutsideRenewalPeriod
        = HouseHoldPolicy.CreateNewSale(new PolicySaleRequest { StartDate = DateOnly.Parse("2025-06-01") });

    public static readonly HouseHoldPolicy MissingPolicy = new() { Reference = Guid.NewGuid() };
}
