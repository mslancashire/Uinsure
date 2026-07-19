using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Uinsure.Core.Models;
using Uinsure.Core.Models.PolicySale;
using Uinsure.Core.Repositories;

namespace Uinsure.Policy.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PolicyController : ControllerBase
{
    private readonly IPolicyRepository _repository;

    public PolicyController(IPolicyRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Makes a household insurance policy sale.
    /// </summary>
    /// <returns></returns>
    /// <response code="201">Created a policy sale.</response>
    /// <response code="400">If provided input is invalid.</response>
    /// <response code="403">If not authorised, ensure all headers are provided.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    public IActionResult MakePolicySale(PolicySaleRequest saleRequest)
    {
        var policy = HouseHoldPolicy.CreateNewSale(saleRequest);

        var result = _repository.CreatePolicy(policy);

        var response = result.Match<IActionResult>(
            newPolicy => CreatedAtAction(nameof(GetPolicyById), new { Version = $"{1}", PolicyReference = newPolicy.Reference }, null),
            failure => BadRequest($"Policy already created.")
        );

        return response;
    }

    /// <summary>
    /// Gets the details of a household insurance policy using an id.
    /// </summary>
    /// <returns></returns>
    /// <response code="201">Created a policy sale.</response>
    /// <response code="400">If provided input is invalid.</response>
    /// <response code="403">If not authorised, ensure all headers are provided.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet]
    [Route("{policyReference:guid}")]
    [Produces(typeof(object))]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    public IActionResult GetPolicyById([FromRoute] Guid policyReference)
    {
        var result = _repository.Get(policyReference);
        var response = result.Match<IActionResult>(
           Ok,
           failure => NotFound($"Policy with reference {policyReference} not found.")
        );

        return response;
    }
}
