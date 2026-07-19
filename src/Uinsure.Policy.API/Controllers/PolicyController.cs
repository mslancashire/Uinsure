using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Uinsure.Policy.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PolicyController : ControllerBase
{
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
    public IActionResult MakePolicySale()
    {
        return CreatedAtAction(nameof(GetPolicyById), new { Version = $"{1}", PolicyId = Guid.NewGuid() }, null);
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
    [Route("{policyId:guid}")]
    [Produces(typeof(object))]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    public IActionResult GetPolicyById([FromRoute] Guid policyId)
    {
        return Ok(new { PolicyId = policyId });
    }
}
