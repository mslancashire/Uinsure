using OneOf;
using OneOf.Types;
using Uinsure.Core.Models;
using Uinsure.Core.Repositories;

namespace Uinsure.Data;

public class StaticPolicyRepository : IPolicyRepository
{
    private static readonly Dictionary<Guid, HouseHoldPolicy> _policies = [];
    
    public OneOf<HouseHoldPolicy, No> CreatePolicy(HouseHoldPolicy policy)
    {
        if (_policies.TryAdd(policy.Reference, policy))
        {
            return policy;
        }

        return new No();
    }

    public OneOf<HouseHoldPolicy, None> Get(Guid policyReference)
    {
        if (_policies.TryGetValue(policyReference, out var policy))
        {
            return policy;
        }

        return new None();
    }
}
