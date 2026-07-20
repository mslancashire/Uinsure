using OneOf;
using OneOf.Types;
using Uinsure.Core.Models;
using Uinsure.Core.Repositories;

namespace Tests.Uinsure.Integration.Services;

internal class TestPolicyRepository : IPolicyRepository
{
    internal static readonly Dictionary<Guid, HouseHoldPolicy> PoliciesStore
        = Fakes.FakeHouseholdPolicies.ExistingPolicies.ToDictionary(k => k.Reference, v => v);

    public OneOf<HouseHoldPolicy, No> CreatePolicy(HouseHoldPolicy policy)
    {
        if (PoliciesStore.TryAdd(policy.Reference, policy) == false)
        {
            return new No();
        }

        return policy;
    }

    public OneOf<HouseHoldPolicy, None> Get(Guid policyReference)
    {
        if (PoliciesStore.TryGetValue(policyReference, out var policy))
        {
            return policy;
        }

        return new None();
    }

    public OneOf<HouseHoldPolicy, None> Update(HouseHoldPolicy policy)
    {
        if (PoliciesStore.ContainsKey(policy.Reference) == false)
        {
            return new None();
        }

        PoliciesStore[policy.Reference] = policy;

        return policy;
    }
}
