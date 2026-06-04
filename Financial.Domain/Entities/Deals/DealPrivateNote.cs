public class DealPrivateNote
{
    public Guid Id { get; set; }

    public Guid DealId { get; set; }

    public Guid UserId { get; set; }

    public string Note { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}