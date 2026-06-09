public class Client
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public ClientType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? INN { get; set; }
    public string? Email { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public Organization Organization { get; set; } = null!;
    public ICollection<Lead> Leads { get; set; }
        = new List<Lead>();
    public ICollection<Deal> Deals { get; set; }
        = new List<Deal>();
}