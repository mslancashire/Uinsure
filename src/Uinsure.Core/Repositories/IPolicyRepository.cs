using OneOf;
using OneOf.Types;
using Uinsure.Core.Models;

namespace Uinsure.Core.Repositories;

/// <summary>
/// Using a interface for the repository to allow for different implementations, e.g. a static repository for testing, a database (sql, pgsql, mongo) repository for production, etc...
/// </summary>
public interface IPolicyRepository
{
    /// <summary>
    /// Creates a new policy in the repository. If the policy already exists, it will return No.
    /// </summary>
    /// <param name="policy"></param>
    /// <returns></returns>
    OneOf<HouseHoldPolicy,No> CreatePolicy(HouseHoldPolicy policy);

    /// <summary>
    /// Gets a policy from the repository. If the policy does not exist, it will return None.
    /// </summary>
    /// <param name="policyReference"></param>
    /// <returns></returns>
    OneOf<HouseHoldPolicy, None> Get(Guid policyReference);
}
