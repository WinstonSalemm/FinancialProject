public class Department
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Organization Organization { get; set; } = null!;
    public ICollection<UserDepartment> UserDepartments { get; set; }
        = new List<UserDepartment>();
}