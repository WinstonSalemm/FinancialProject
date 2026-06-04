public class CalculationVersionItem
{
    public Guid Id { get; set; }

    public Guid CalculationVersionId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public decimal ListPrice { get; set; }

    public decimal PartnerDiscount { get; set; }

    public decimal PartnerPrice { get; set; }
    public CalculationVersion CalculationVersion { get; set; } = null!;
}