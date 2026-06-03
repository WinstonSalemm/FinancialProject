public class Deal
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid ClientId { get; set; }

    public Guid ResponsibleUserId { get; set; }

    public string ContractNumber { get; set; } = string.Empty;

    public Guid LeadId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}