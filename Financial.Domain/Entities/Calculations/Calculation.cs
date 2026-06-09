public class Calculation
{
    public Guid Id { get; set; }
    public Guid LeadId { get; set; }
    public Guid CurrentVersionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Lead Lead { get; set; } = null!;
    public Guid OrganizationId { get; set; }

    public Organization Organization { get; set; } = null!;
    public CalculationVersion CurrentVersion { get; set; } = null!;
    public ICollection<CalculationVersion> Versions { get; set; }
        = new List<CalculationVersion>();
}