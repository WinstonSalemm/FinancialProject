public class LeadHistory
{
    public Guid Id { get; set; }

    public Guid LeadId { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Action { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}