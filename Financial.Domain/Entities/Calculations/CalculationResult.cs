public class CalculationResult
{
    public Guid Id { get; set; }
    public Guid CalculationVersionId { get; set; }
    public decimal TotalCostWithoutVat { get; set; }
    public decimal CustomerPriceWithoutVat { get; set; }
    public decimal VatAmount { get; set; }
    public decimal CustomerPriceWithVat { get; set; }
    public decimal ProfitTaxAmount { get; set; }
    public decimal DividendTaxAmount { get; set; }
    public decimal MarginPercent { get; set; }
    public decimal AvailableDividendAmount { get; set; }
}