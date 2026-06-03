public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string SecondName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid RoleId { get; set; } = Guid.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}