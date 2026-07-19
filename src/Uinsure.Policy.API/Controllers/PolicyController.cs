using Asp.Versioning;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Uinsure.Core.Models;
using Uinsure.Core.Repositories;

namespace Uinsure.Policy.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class PolicyController : ControllerBase
{
    private readonly IPolicyRepository _repository;
    private readonly IValidator<HouseHoldPolicy> _renewalValidator;

    public PolicyController(IPolicyRepository repository, IValidator<HouseHoldPolicy> renewalValidator)
    {
        _repository = repository;
        _renewalValidator = renewalValidator;
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

    /// <summary>
    /// Gets the details of a household insurance policy using an id.
    /// </summary>
    /// <returns></returns>
    /// <response code="201">Created a policy sale.</response>
    /// <response code="400">If provided input is invalid.</response>
    /// <response code="403">If not authorised, ensure all headers are provided.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPut]
    [Route("{policyReference:guid}/renew")]
    [Produces(typeof(object))]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    public IActionResult RenewPolicy([FromRoute] Guid policyReference)
    {
        var result = _repository.Get(policyReference);
        if (result.TryPickT0(out var policy, out _))
        {
            return NotFound($"Policy with reference {policyReference} not found.");
        }

        var readyForRenewal = _renewalValidator.Validate(policy);
        if (readyForRenewal.IsValid == false)
        {
            return BadRequest(ProblemDetails(readyForRenewal, HttpContext));
        }

        var renewedPolicy = policy.Renew();

        return Ok(renewedPolicy);
    }

    public static ProblemDetails ProblemDetails(ValidationResult validationResult, HttpContext context)
    {
        return CreateProblemDetails(validationResult.ToDictionary(), context);
    }

    private static HttpValidationProblemDetails CreateProblemDetails(IDictionary<string, string[]> errors, HttpContext context)
    {
        var problemOutput = new HttpValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation failed",
            Detail = "One or more validation errors occurred.",
            Instance = context.Request.Path,
        };

        problemOutput.Extensions.TryAdd("traceId", context.TraceIdentifier);

        return problemOutput;
    }
}
