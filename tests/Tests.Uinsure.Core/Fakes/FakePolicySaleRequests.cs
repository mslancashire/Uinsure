using Uinsure.Core.Models;

namespace Tests.Uinsure.Core.Fakes;

public class FakePolicySaleRequests
{
    private readonly TimeProvider _timeProvider;
    private readonly DateOnly _validStartDate;
    private readonly DateOnly _now;

    public FakePolicySaleRequests(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        _now = DateOnly.FromDateTime(_timeProvider.GetUtcNow().Date);
        _validStartDate = _now.AddDays(5);
    }

    public PolicySaleRequest MissingStartDate()
        => new();

    public PolicySaleRequest Valid()
        => new()
        {
            StartDate = _validStartDate,
            PolicyHolders = [FakePolicyHolders.Over16(_validStartDate)],
            Property = Property.Basic("10 Test Street", "TE5 5ST")
        };

    public PolicySaleRequest ForToday()
        => new()
        {
            StartDate = _now,
            PolicyHolders = [FakePolicyHolders.Over16(_now)],
            Property = Property.Basic("10 Test Street", "TE5 5ST")
        };

    public PolicySaleRequest InPast()
        => new() { StartDate = _now.AddDays(-1) };

    public PolicySaleRequest Over60Days()
        => new() { StartDate = _now.AddDays(62) };

    public PolicySaleRequest Exactly60Days()
        => new() { StartDate = _now.AddDays(60) };

    public PolicySaleRequest NoHolders()
        => new() { StartDate = _now.AddDays(5) };

    public PolicySaleRequest MoreThen3Holders()
        => new()
        {
            StartDate = _now,
            PolicyHolders = [FakePolicyHolders.Over16(_now), FakePolicyHolders.Over16(_now), FakePolicyHolders.Over16(_now), FakePolicyHolders.Over16(_now)]
        };

    public PolicySaleRequest YoungerThan16()
        => new()
        {
            StartDate = _validStartDate.AddDays(5),
            PolicyHolders = [FakePolicyHolders.Over16(_validStartDate), FakePolicyHolders.Under16(_validStartDate)]
        };

    public PolicySaleRequest JustUnder16()
        => new()
        {
            StartDate = _validStartDate,
            PolicyHolders = [FakePolicyHolders.JustUnder16(_validStartDate), FakePolicyHolders.Over16(_validStartDate)]
        };

    public PolicySaleRequest NoProperty()
        => new()
        {
            StartDate = _validStartDate,
            PolicyHolders = [FakePolicyHolders.Over16(_validStartDate)],
        };
}
