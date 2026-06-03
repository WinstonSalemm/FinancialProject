public enum ApprovalStatus
{
    Pending,
    Approved,
    Rejected
}

public class ApprovalRequest
{
    public Guid Id { get; set; }

    public Guid LeadId { get; set; }

    public Guid RequestedByUserId { get; set; }

    public Guid ApproverUserId { get; set; }

    public ApprovalStatus Status { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ApprovedAt { get; set; }
}