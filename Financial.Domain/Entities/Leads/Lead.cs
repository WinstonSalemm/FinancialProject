public class Lead
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid ClientId { get; set; }
    public Guid ResponsibleUserId { get; set; }
    public Guid CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public LeadStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Organization Organization { get; set; } = null!;
    public DateTime? WonAt { get; set; }
    public DateTime? LostAt { get; set; }
    public Client Client { get; set; } = null!;
    public User ResponsibleUser { get; set; } = null!;
    public ICollection<Calculation> Calculations { get; set; }
        = new List<Calculation>();
    public ICollection<LeadHistory> History { get; set; }
        = new List<LeadHistory>();
    public ICollection<LeadPrivateNote> PrivateNotes { get; set; }
        = new List<LeadPrivateNote>();
}