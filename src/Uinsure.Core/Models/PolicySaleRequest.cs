namespace Uinsure.Core.Models;

/// <summary>
/// A request to make a house hold policy sale.
/// </summary>
public class PolicySaleRequest
{
    /// <summary>
    /// The date the policy is to start.
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// Who are the holders of the policy.
    /// </summary>
    public IEnumerable<PolicyHolder> PolicyHolders { get; set; } = [];

    /// <summary>
    /// Which property should the policy cover.
    /// </summary>
    public Property? Property { get; set; }

    /// <summary>
    /// How will the policy be paid for.
    /// </summary>
    public PaymentType PaymentType { get; set; } = PaymentType.None;

    /// <summary>
    /// How much is the policy for.
    /// </summary>
    public decimal Price { get; set; } = 0;
}
