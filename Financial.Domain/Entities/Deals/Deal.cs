public class Deal
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid ClientId { get; set; }
    public Guid ResponsibleUserId { get; set; }
    public Guid LeadId { get; set; }
    public Guid CalculationVersionId { get; set; }
    public string ContractNumber { get; set; } = string.Empty;
    public Guid CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;
    public DealStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Organization Organization { get; set; } = null!;
    public Client Client { get; set; } = null!;
    public User ResponsibleUser { get; set; } = null!;
    public Lead Lead { get; set; } = null!;
    public CalculationVersion CalculationVersion { get; set; } = null!;
    public ICollection<DealHistory> History { get; set; }
        = new List<DealHistory>();
    public ICollection<DealComment> Comments { get; set; }
        = new List<DealComment>();
    public ICollection<DealPrivateNote> PrivateNotes { get; set; }
        = new List<DealPrivateNote>();
}