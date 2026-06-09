public class LeadHistory
{
    public Guid Id { get; set; }
    public Guid LeadId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Comment { get; set; }
    public User User { get; set; } = null!;
    public Lead Lead { get; set; } = null!;
    public LeadHistoryEventType EventType { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}