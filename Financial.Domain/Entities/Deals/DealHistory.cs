public class DealHistory
{
    public Guid Id { get; set; }
    public Guid DealId { get; set; }
    public DealStatus FromStatus { get; set; }
    public DealStatus ToStatus { get; set; }
    public Guid ChangedByUserId { get; set; }
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; }
    public Deal Deal { get; set; } = null!;
    public User ChangedByUser { get; set; } = null!;
}