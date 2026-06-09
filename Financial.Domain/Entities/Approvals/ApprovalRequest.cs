public class ApprovalRequest
{
    public Guid Id { get; set; }
    public Guid CalculationVersionId { get; set; }
    public Guid RequestedByUserId { get; set; }
    public Guid ApproverUserId { get; set; }
    public ApprovalStatus Status { get; set; }
    public string? Comment { get; set; }
    public Organization Organization { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public CalculationVersion CalculationVersion { get; set; } = null!;
    public User RequestedByUser { get; set; } = null!;
    public User ApproverUser { get; set; } = null!;
}