public class CalculationVersion
{
    public Guid Id { get; set; }
    public Guid CalculationId { get; set; }
    public int VersionNumber { get; set; }
    public decimal ExchangeRate { get; set; }
    public decimal RoyaltyPercent { get; set; }
    public decimal RiskPercent { get; set; }
    public decimal BankExpensePercent { get; set; }
    public decimal MarkupPercent { get; set; }
    public decimal VatPercent { get; set; }
    public decimal ProfitTaxPercent { get; set; }
    public decimal DividendTaxPercent { get; set; }
    public CalculationVersionStatus Status { get; set; } = CalculationVersionStatus.Draft;
    public Guid CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Calculation Calculation { get; set; } = null!;
    public User CreatedByUser { get; set; } = null!;
    public ICollection<CalculationVersionItem> Items { get; set; }
        = new List<CalculationVersionItem>();
    public ICollection<ApprovalRequest> ApprovalRequests { get; set; }
        = new List<ApprovalRequest>();
    public CalculationVersionResult? Result { get; set; }
}