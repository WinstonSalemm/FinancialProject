public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SecondName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid RoleId { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? Position { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public Organization Organization { get; set; } = null!;
    public Role Role { get; set; } = null!;
    public ICollection<UserDepartment> UserDepartments { get; set; }
    = new List<UserDepartment>();
    public ICollection<Lead> Leads { get; set; }
    = new List<Lead>();
    public ICollection<Deal> Deals { get; set; }
        = new List<Deal>();
}