public class Organization
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string INN { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public ICollection<User> Users { get; set; }
    = new List<User>();
    public ICollection<Client> Clients { get; set; }
        = new List<Client>();
    public ICollection<Lead> Leads { get; set; }
        = new List<Lead>();
    public ICollection<Deal> Deals { get; set; }
        = new List<Deal>();
    public ICollection<Department> Departments { get; set; }
        = new List<Department>();
}
